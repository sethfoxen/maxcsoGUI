# Building maxcsoGUI

## Prerequisites

- **Visual Studio 2022** (Community or higher)
  - Workload: **Desktop development with C++**
  - Individual component: **.NET Framework 4.8 development tools** (or the full .NET Framework targeting pack)
- **maxcso source** cloned as a sibling directory named `maxcso/`, with its submodules initialised:
  ```
  git clone --recursive https://github.com/unknownbrackets/maxcso.git
  ```
  The bridge project references `..\..\maxcso\src\` and `..\..\maxcso\libuv\`, so the layout must be:
  ```
  repos/
    maxcso/          ← sibling clone with submodules
    maxcsoGUI/       ← this repo
  ```

---

## Building

### Option A — Visual Studio IDE

1. Open `maxcsoGUI.sln`.
2. Set the solution configuration to **Release** and the platform to **x64**.
3. Build → Build Solution (`Ctrl+Shift+B`).

### Option B — MSBuild command line

```powershell
$msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
& $msbuild maxcsoGUI.sln /p:Configuration=Release "/p:Platform=x64" /v:minimal
```

---

## Output locations

| Artifact | Path |
|---|---|
| GUI executable | `maxcsoGUI\bin\Release\maxcsoGUI.exe` |
| Bridge DLL (x64) | `maxcsoBridge\bin\Release\native\x64\maxcsoBridge.dll` |

The GUI searches several candidate paths for the DLL at runtime; `native\x64\` relative to the exe is one of them.

---

## ABI versioning

The bridge DLL exports `MaxcsoBridgeGetVersion()` which returns the integer constant `MAXCSO_BRIDGE_VERSION` defined in `maxcsoBridge.h`. The GUI checks this at load time and refuses to use a DLL whose version does not match `ExpectedBridgeVersion` in `MaxcsoNative.vb`.

A version mismatch surfaces as a **"Conversion Failed"** dialog — not a silent fallback — so it is always visible.

### When you change the bridge ABI (request struct layout or exported signatures)

1. Increment `MAXCSO_BRIDGE_VERSION` in `maxcsoBridge\maxcsoBridge.h`.
2. Increment `ExpectedBridgeVersion` in `maxcsoGUI\MaxcsoNative.vb` to the same value.
3. Update the `NativeBridgeRequest` struct in `MaxcsoNative.vb` to match the C++ struct field-for-field (same order, same types).
4. Rebuild both projects.

### Verifying a clean build

After building, start the GUI and attempt a conversion. If you do **not** see a version-mismatch error in the failure dialog, the DLL and the GUI are in sync.
