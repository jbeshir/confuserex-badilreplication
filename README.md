# confuserex-badilreplication
Sample project to replicate a ConfuserEx bug resulting in an invalid program.

Demonstrates a bug in ConfuserEx's switch-based control flow obfuscation in which the following IL:

	.method public hidebysig 
		instance void RaiseChanged (
			int32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2115
		// Code size 40 (0x28)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class ConfuserExTestCase.TestClassEventArgs ConfuserExTestCase.TestClass::_sharedEventArgs
		IL_0007: ldarg.1
		IL_0008: callvirt instance void ConfuserExTestCase.TestClassEventArgs::Set(int32)
		IL_000d: nop
		IL_000e: ldarg.0
		IL_000f: ldfld class [mscorlib]System.EventHandler`1<class ConfuserExTestCase.TestClassEventArgs> ConfuserExTestCase.TestClass::Changed
		IL_0014: dup
		IL_0015: brtrue.s IL_001a

		IL_0017: pop
		IL_0018: br.s IL_0027

		IL_001a: ldarg.0
		IL_001b: ldarg.0
		IL_001c: ldfld class ConfuserExTestCase.TestClassEventArgs ConfuserExTestCase.TestClass::_sharedEventArgs
		IL_0021: callvirt instance void class [mscorlib]System.EventHandler`1<class ConfuserExTestCase.TestClassEventArgs>::Invoke(object, !0)
		IL_0026: nop

		IL_0027: ret
	} // end of method TestClass::RaiseChanged

Is incorrectly compiled to the following IL, which is invalid because IL_0065 jumps to IL_0033 with a value on the evaluation stack, but the switch statement at IL_0010 goes there with none.

	.method public hidebysig 
		instance void RaiseChanged (
			int32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2268
		// Code size 169 (0xa9)
		.maxstack 5
		.locals init (
			[0] uint32
		)

		IL_0000: nop

		IL_0001: ldc.i4 -11784292
		// loop start (head: IL_0006)
			IL_0006: ldc.i4 -1661060488
			IL_000b: xor
			IL_000c: dup
			IL_000d: stloc.0
			IL_000e: ldc.i4.7
			IL_000f: rem.un
			IL_0010: switch (IL_005e, IL_0033, IL_00a8, IL_0095, IL_0077, IL_0001, IL_004e)

			IL_0031: br.s IL_00a8

			IL_0033: ldarg.0
			IL_0034: ldarg.0
			IL_0035: ldfld class ConfuserExTestCase.TestClassEventArgs ConfuserExTestCase.TestClass::_sharedEventArgs
			IL_003a: callvirt instance void class [mscorlib]System.EventHandler`1<class ConfuserExTestCase.TestClassEventArgs>::Invoke(object, !0)
			IL_003f: ldloc.0
			IL_0040: ldc.i4 -810392159
			IL_0045: mul
			IL_0046: ldc.i4 1279325931
			IL_004b: xor
			IL_004c: br.s IL_0006

			IL_004e: nop
			IL_004f: ldloc.0
			IL_0050: ldc.i4 -916224833
			IL_0055: mul
			IL_0056: ldc.i4 -584222168
			IL_005b: xor
			IL_005c: br.s IL_0006

			IL_005e: ldarg.0
			IL_005f: ldfld class [mscorlib]System.EventHandler`1<class ConfuserExTestCase.TestClassEventArgs> ConfuserExTestCase.TestClass::Changed
			IL_0064: dup
			IL_0065: brtrue.s IL_0033

			IL_0067: pop
			IL_0068: ldloc.0
			IL_0069: ldc.i4 -1643779416
			IL_006e: mul
			IL_006f: ldc.i4 553812884
			IL_0074: xor
			IL_0075: br.s IL_0006

			IL_0077: ldarg.0
			IL_0078: ldfld class ConfuserExTestCase.TestClassEventArgs ConfuserExTestCase.TestClass::_sharedEventArgs
			IL_007d: ldarg.1
			IL_007e: callvirt instance void ConfuserExTestCase.TestClassEventArgs::Set(int32)
			IL_0083: ldloc.0
			IL_0084: ldc.i4 -1552395833
			IL_0089: mul
			IL_008a: ldc.i4 -1804021620
			IL_008f: xor
			IL_0090: br IL_0006

			IL_0095: nop
			IL_0096: ldloc.0
			IL_0097: ldc.i4 758969978
			IL_009c: mul
			IL_009d: ldc.i4 -778064956
			IL_00a2: xor
			IL_00a3: br IL_0006
		// end loop

		IL_00a8: ret
	} // end of method TestClass::RaiseChanged

This includes sample C# 6 code which generates the input IL when compiled (at least in debug mode) as a result of use of the null propagation operator.
