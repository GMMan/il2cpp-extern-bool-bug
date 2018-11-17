#pragma once

#if _WINDLL
#define DLLTAG extern "C" __declspec(dllexport)
#else
#define DLLTAG extern "C" __declspec(dllimport)
#endif

DLLTAG int TestFunc();
