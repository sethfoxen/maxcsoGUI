#include "maxcsoBridge.h"

#include <algorithm>
#include <cinttypes>
#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <string>
#include <vector>

#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif
#include <winsock2.h>
#include <ws2tcpip.h>
#include <windows.h>

#include "..\..\maxcso\src\checksum.h"
#include "..\..\maxcso\src\compress.h"
#include "..\..\maxcso\libuv\include\uv.h"

namespace {

std::string WideToUtf8(const wchar_t *value) {
	if (value == nullptr || *value == L'\0') {
		return std::string();
	}

	int bytes = WideCharToMultiByte(CP_UTF8, 0, value, -1, nullptr, 0, nullptr, nullptr);
	if (bytes <= 1) {
		return std::string();
	}

	std::string result(bytes - 1, '\0');
	WideCharToMultiByte(CP_UTF8, 0, value, -1, &result[0], bytes, nullptr, nullptr);
	return result;
}

std::wstring Utf8ToWide(const std::string &value) {
	if (value.empty()) {
		return std::wstring();
	}

	int chars = MultiByteToWideChar(CP_UTF8, 0, value.c_str(), -1, nullptr, 0);
	if (chars <= 1) {
		return std::wstring();
	}

	std::wstring result(chars - 1, L'\0');
	MultiByteToWideChar(CP_UTF8, 0, value.c_str(), -1, &result[0], chars);
	return result;
}

void SetMessageBuffer(wchar_t *messageBuffer, int messageBufferChars, const std::wstring &message) {
	if (messageBuffer == nullptr || messageBufferChars <= 0) {
		return;
	}

	wcsncpy_s(messageBuffer, static_cast<size_t>(messageBufferChars), message.c_str(), _TRUNCATE);
}

void ReportProgress(MaxcsoBridgeProgressCallback progressCallback, void *userData, int percent, const std::wstring &message) {
	if (progressCallback == nullptr) {
		return;
	}

	progressCallback(percent, message.c_str(), userData);
}

uint32_t BuildFlags(const MaxcsoBridgeRequest *request, std::wstring &validationError) {
	uint32_t flagsFmt = 0;
	switch (request->format) {
	case MAXCSO_BRIDGE_FMT_CSO2:
		flagsFmt = maxcso::TASKFLAG_FMT_CSO_2;
		break;
	case MAXCSO_BRIDGE_FMT_ZSO:
		flagsFmt = maxcso::TASKFLAG_FMT_ZSO;
		break;
	case MAXCSO_BRIDGE_FMT_DAX:
		flagsFmt = maxcso::TASKFLAG_FMT_DAX;
		break;
	case MAXCSO_BRIDGE_FMT_CSO1:
	default:
		flagsFmt = 0;
		break;
	}

	uint32_t flagsFinal = 0;
	if (!request->use_zlib) {
		flagsFinal |= maxcso::TASKFLAG_NO_ZLIB | maxcso::TASKFLAG_NO_ZLIB_DEFAULT | maxcso::TASKFLAG_NO_ZLIB_BRUTE;
	}
	if (!request->use_zopfli) {
		flagsFinal |= maxcso::TASKFLAG_NO_ZOPFLI;
	}
	if (!request->use_7zdeflate) {
		flagsFinal |= maxcso::TASKFLAG_NO_7ZIP;
	}
	if (!request->use_lz4) {
		flagsFinal |= maxcso::TASKFLAG_NO_LZ4 | maxcso::TASKFLAG_NO_LZ4_HC;
	}
	if (!request->use_lz4brute) {
		flagsFinal |= maxcso::TASKFLAG_NO_LZ4_HC_BRUTE;
	}
	if (!request->use_libdeflate) {
		flagsFinal |= maxcso::TASKFLAG_NO_LIBDEFLATE;
	}

	// Defense in depth: format-incompatible methods are forced off even if the caller asked for them.
	if (flagsFmt & maxcso::TASKFLAG_FMT_ZSO) {
		flagsFinal |= maxcso::TASKFLAG_NO_ZLIB | maxcso::TASKFLAG_NO_ZLIB_DEFAULT | maxcso::TASKFLAG_NO_ZLIB_BRUTE |
			maxcso::TASKFLAG_NO_ZOPFLI | maxcso::TASKFLAG_NO_7ZIP | maxcso::TASKFLAG_NO_LIBDEFLATE;
	} else if ((flagsFmt & maxcso::TASKFLAG_FMT_CSO_2) == 0) {
		flagsFinal |= maxcso::TASKFLAG_NO_LZ4 | maxcso::TASKFLAG_NO_LZ4_HC | maxcso::TASKFLAG_NO_LZ4_HC_BRUTE;
	}

	if (request->fast) {
		flagsFinal |= maxcso::TASKFLAG_NO_ZLIB_BRUTE | maxcso::TASKFLAG_NO_ZOPFLI | maxcso::TASKFLAG_NO_7ZIP | maxcso::TASKFLAG_NO_LZ4_HC_BRUTE | maxcso::TASKFLAG_NO_LZ4_HC | maxcso::TASKFLAG_NO_LIBDEFLATE;
	}
	if (request->decompress) {
		flagsFinal |= maxcso::TASKFLAG_DECOMPRESS;
	}
	if (request->measure_only) {
		flagsFinal |= maxcso::TASKFLAG_MEASURE;
	}
	flagsFinal |= flagsFmt;

	if (flagsFmt & maxcso::TASKFLAG_FMT_DAX) {
		if (request->block_size_enabled) {
			validationError = L"Block size must be default for DAX.";
			return 0;
		}

		uint32_t deflateFlags = maxcso::TASKFLAG_NO_ZLIB | maxcso::TASKFLAG_NO_ZLIB_DEFAULT | maxcso::TASKFLAG_NO_ZLIB_BRUTE | maxcso::TASKFLAG_NO_ZOPFLI | maxcso::TASKFLAG_NO_7ZIP | maxcso::TASKFLAG_NO_LIBDEFLATE;
		if ((flagsFinal & deflateFlags) == deflateFlags && !request->decompress) {
			validationError = L"DAX must use some kind of DEFLATE.";
			return 0;
		}
	}

	return flagsFinal;
}

void UpdateThreadpoolSize(int threads) {
	char threadpoolSize[32];
	sprintf_s(threadpoolSize, "%d", threads);
	_putenv_s("UV_THREADPOOL_SIZE", threadpoolSize);
}

}  // namespace

extern "C" MAXCSOBRIDGE_API int __stdcall MaxcsoBridgeGetVersion() {
	return MAXCSO_BRIDGE_VERSION;
}

extern "C" MAXCSOBRIDGE_API int __stdcall MaxcsoBridgeProcess(const MaxcsoBridgeRequest *request, MaxcsoBridgeProgressCallback progressCallback, void *userData, wchar_t *messageBuffer, int messageBufferChars) {
	if (request == nullptr) {
		SetMessageBuffer(messageBuffer, messageBufferChars, L"No request was provided.");
		return 1;
	}

	if (request->input_path == nullptr || *request->input_path == L'\0') {
		SetMessageBuffer(messageBuffer, messageBufferChars, L"No input file was provided.");
		return 1;
	}

	if (!request->crc_only && !request->measure_only && !request->decompress && request->output_path != nullptr && *request->output_path == L'\0') {
		SetMessageBuffer(messageBuffer, messageBufferChars, L"No output file was provided.");
		return 1;
	}

	int threads = request->threads;
	if (threads <= 0) {
		uv_cpu_info_t *cpus = nullptr;
		if (uv_cpu_info(&cpus, &threads) != 0 || threads <= 0) {
			threads = 1;
		}
		if (cpus != nullptr) {
			uv_free_cpu_info(cpus, threads);
		}
	}

	std::wstring validationError;
	uint32_t flags = BuildFlags(request, validationError);
	if (!validationError.empty()) {
		SetMessageBuffer(messageBuffer, messageBufferChars, validationError);
		return 1;
	}

	UpdateThreadpoolSize(threads);

	maxcso::Task task;
	task.input = WideToUtf8(request->input_path);
	if (!request->crc_only && !request->measure_only && request->output_path != nullptr) {
		task.output = WideToUtf8(request->output_path);
	}
	task.block_size = request->block_size_enabled ? request->block_size : maxcso::DEFAULT_BLOCK_SIZE;
	task.flags = flags;
	task.orig_max_cost_percent = request->orig_cost_enabled ? request->orig_cost_percent : 0.0;
	task.lz4_max_cost_percent = request->lz4_cost_enabled ? request->lz4_cost_percent : 0.0;

	int result = 0;
	std::wstring finalMessage;
	int lastPercent = -1;

	task.progress = [&](const maxcso::Task *, maxcso::TaskStatus status, int64_t pos, int64_t total, int64_t written) {
		if (status != maxcso::TASK_SUCCESS && status != maxcso::TASK_INPROGRESS) {
			return;
		}

		double ratio = total <= 0 ? 0.0 : (pos * 100.0) / total;
		int percent = static_cast<int>(ratio);
		if (status == maxcso::TASK_SUCCESS || percent > 100) {
			percent = 100;
		}

		char buffer[128];
		sprintf_s(buffer, "%" PRId64 " / %" PRId64 " bytes (%.0f%%)", pos, total, ratio);
		finalMessage = Utf8ToWide(buffer);

		if (percent != lastPercent || status == maxcso::TASK_SUCCESS) {
			lastPercent = percent;
			ReportProgress(progressCallback, userData, percent, finalMessage);
		}
	};

	task.error = [&](const maxcso::Task *, maxcso::TaskStatus status, const char *reason) {
		if (status != maxcso::TASK_SUCCESS) {
			result = 1;
		}

		if (reason != nullptr) {
			finalMessage = Utf8ToWide(reason);
		} else if (status != maxcso::TASK_SUCCESS) {
			finalMessage = L"The maxcso library reported an error.";
		}
	};

	std::vector<maxcso::Task> tasks;
	tasks.push_back(task);

	if (request->crc_only) {
		maxcso::Checksum(tasks);
	} else {
		maxcso::Compress(tasks);
	}

	if (result == 0 && lastPercent < 100) {
		ReportProgress(progressCallback, userData, 100, finalMessage);
	}

	SetMessageBuffer(messageBuffer, messageBufferChars, finalMessage);
	return result;
}
