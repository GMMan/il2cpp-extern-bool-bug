il2cpp `bool` Bug for P/Invoke
==============================

This project demonstrates a bug that occurs when compiling a game with il2cpp.
It changes `false` return values from the native DLL into `true` when returned
back to the game.

Demonstration
-------------
Compile a il2cpp version of the game and a Mono version of the game. Run both.
The il2cpp version will say "True", while the Mono version will say "False".
The DLL returns a value that is effectively `false`, so the Mono version is
correct and the il2cpp version is wrong.

Analysis
--------
Inspect the following C++ code generated for the `extern` function
`NativeImport.TestFunc`:

```cpp
// System.Boolean NativeImport::TestFunc()
extern "C" IL2CPP_METHOD_ATTR bool NativeImport_TestFunc_m1408486199 (RuntimeObject * __this /* static, unused */, const RuntimeMethod* method)
{
	typedef int32_t (DEFAULT_CALL *PInvokeFunc) ();
	static PInvokeFunc il2cppPInvokeFunc;
	if (il2cppPInvokeFunc == NULL)
	{
		int parameterSize = 0;
		il2cppPInvokeFunc = il2cpp_codegen_resolve_pinvoke<PInvokeFunc>(IL2CPP_NATIVE_STRING("TestDll"), "TestFunc", IL2CPP_CALL_DEFAULT, CHARSET_NOT_SPECIFIED, parameterSize, false);

		if (il2cppPInvokeFunc == NULL)
		{
			IL2CPP_RAISE_MANAGED_EXCEPTION(il2cpp_codegen_get_not_supported_exception("Unable to find method for p/invoke: 'TestFunc'"), NULL, NULL);
		}
	}

	// Native function invocation
	int32_t returnValue = il2cppPInvokeFunc();

	return static_cast<bool>(returnValue);
}
```

Look specifically at the last two lines.

By convention, a `bool` value is returned in the lowest 8 bits of the return value
register (i.e. `AL`). il2cpp, however, instead of declaring the return value as
a `bool`, declares it as `int32_t`. This will take the full 32-bit value from the
return register (i.e. `EAX`). With the `static_cast<bool>` performed, any bit set
to `1` in `returnValue` will cause the casted value to become `true`. In our case,
our return value is `0xffffff00`, which casts to a true value, even though the
actual boolean value should resolve to false.

Remedy
------
il2cpp should either store the return value as a `bool`, or mask the lowest 8 bits
so the rest of the bits do not affect the final value.
