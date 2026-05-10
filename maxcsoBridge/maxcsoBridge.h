#pragma once

#ifdef MAXCSOBRIDGE_EXPORTS
#define MAXCSOBRIDGE_API __declspec(dllexport)
#else
#define MAXCSOBRIDGE_API __declspec(dllimport)
#endif

enum MaxcsoBridgeFormat {
	MAXCSO_BRIDGE_FMT_CSO1 = 0,
	MAXCSO_BRIDGE_FMT_CSO2 = 1,
	MAXCSO_BRIDGE_FMT_ZSO = 2,
	MAXCSO_BRIDGE_FMT_DAX = 3,
};

typedef void (__stdcall *MaxcsoBridgeProgressCallback)(int percent, const wchar_t *message, void *user_data);

struct MaxcsoBridgeRequest {
	const wchar_t *input_path;
	const wchar_t *output_path;
	int threads;
	MaxcsoBridgeFormat format;
	int fast;
	int use_zopfli;
	int use_libdeflate;
	int use_lz4brute;
	int decompress;
	int crc_only;
	int measure_only;
	int block_size_enabled;
	unsigned int block_size;
	int orig_cost_enabled;
	double orig_cost_percent;
	int lz4_cost_enabled;
	double lz4_cost_percent;
};

extern "C" MAXCSOBRIDGE_API int __stdcall MaxcsoBridgeProcess(const MaxcsoBridgeRequest *request, MaxcsoBridgeProgressCallback progressCallback, void *userData, wchar_t *messageBuffer, int messageBufferChars);
