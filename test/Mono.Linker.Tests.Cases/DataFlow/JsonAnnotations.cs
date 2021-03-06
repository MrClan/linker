﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Mono.Linker.Tests.Cases.Expectations.Assertions;
using Mono.Linker.Tests.Cases.Expectations.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Mono.Linker.Tests.Cases.DataFlow
{
	[SkipKeptItemsValidation]
	[SetupLinkerAttributeDefinitionsFile ("JsonAnnotations.json")]
	class JsonAnnotations
	{
		public static void Main ()
		{
			var instance = new JsonAnnotations ();

			instance.ReadFromInstanceField ();
			instance.TwoAnnotatedParameters (typeof (TestType), typeof (TestType));
			instance.ReturnConstructorsFailure (null);
			instance.ReadFromInstanceProperty ();
		}

		Type _typeWithDefaultConstructor;

		Type PropertyWithDefaultConstructor { get; set; }

		[UnrecognizedReflectionAccessPattern (typeof (JsonAnnotations), nameof (RequirePublicConstructors), new Type[] { typeof (Type) })]
		[UnrecognizedReflectionAccessPattern (typeof (JsonAnnotations), nameof (RequireNonPublicConstructors), new Type[] { typeof (Type) })]
		private void ReadFromInstanceField ()
		{
			RequireDefaultConstructor (_typeWithDefaultConstructor);
			RequirePublicConstructors (_typeWithDefaultConstructor);
			RequireNonPublicConstructors (_typeWithDefaultConstructor);
		}

		[UnrecognizedReflectionAccessPattern (typeof (JsonAnnotations), nameof (RequirePublicConstructors), new Type[] { typeof (Type) })]
		private void TwoAnnotatedParameters (
			Type type,
			Type type2)
		{
			RequireDefaultConstructor (type);
			RequireDefaultConstructor (type2);
			RequirePublicConstructors (type);
			RequirePublicConstructors (type2);
		}

		[UnrecognizedReflectionAccessPattern (typeof (JsonAnnotations), nameof (ReturnConstructorsFailure), new Type[] { typeof (Type) },
			returnType: typeof (Type))]
		private Type ReturnConstructorsFailure (
			Type defaultConstructorType)
		{
			return defaultConstructorType;
		}

		[UnrecognizedReflectionAccessPattern (typeof (JsonAnnotations), nameof (RequirePublicConstructors), new Type[] { typeof (Type) })]
		[UnrecognizedReflectionAccessPattern (typeof (JsonAnnotations), nameof (RequireNonPublicConstructors), new Type[] { typeof (Type) })]
		private void ReadFromInstanceProperty ()
		{
			RequireDefaultConstructor (PropertyWithDefaultConstructor);
			RequirePublicConstructors (PropertyWithDefaultConstructor);
			RequireNonPublicConstructors (PropertyWithDefaultConstructor);
		}

		private static void RequireDefaultConstructor (
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.DefaultConstructor)]
			Type type)
		{
		}

		private static void RequirePublicConstructors (
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
			Type type)
		{
		}

		private static void RequireNonPublicConstructors (
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors)]
			Type type)
		{
		}

		class TestType { }
	}
}
