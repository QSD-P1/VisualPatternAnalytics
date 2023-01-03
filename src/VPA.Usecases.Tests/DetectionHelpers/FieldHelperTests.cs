using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class FieldHelperTests
	{
		[Fact]
		public void ClassHasPublicStaticFieldWithOwnType_ShouldReturnTrue()
		{
			var className = "TestClass";

			var fieldNode = new FieldNode()
			{
				Type = className,
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Private
			};

			var classNode = new ClassNode()
			{
				Name = className,
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var result = FieldHelper.ClassHasPrivateStaticFieldWithOwnType(classNode, out var leaf);

			Assert.True(result && leaf != null);
		}

		[Fact]
		public void ClassHasPublicStaticFieldWithOwnType_ShouldReturnFalse()
		{
			var className = "TestClass";

			var fieldNode = new FieldNode()
			{
				Type = className,
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Private
			};

			var classNode = new ClassNode()
			{
				Name = className,
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var result = FieldHelper.ClassHasPrivateStaticFieldWithOwnType(classNode, out var leaf);

			Assert.False(result && leaf == null);
		}

		[Fact]
		public void GetCollectionGenericObject_WithGeneric_ReturnCollectionGenericObject()
		{
			var genericCollectionString = "List<Parent>";
			var result = FieldHelper.GetCollectionGenericObject(genericCollectionString);
			Assert.Equal(result.CollectionType, "List");
			Assert.Equal(result.GenericType, "Parent");
		}

		[Fact]
		public void GetCollectionGenericObject_WithoutGeneric_DontReturnCollectionGenericObject()
		{
			var genericCollectionString = "List";
			var result = FieldHelper.GetCollectionGenericObject(genericCollectionString);
			Assert.Equal(result.CollectionType, "List");
			Assert.Equal(result.GenericType, null);
		}

		[Fact]
		public void GetCollectionGenericObject_WithNullParam_ReturnsNull()
		{
			var result = FieldHelper.GetCollectionGenericObject(null);
			Assert.Null(result);
		}

		[Fact]
		public void GetCollectionGenericObject_WithEmptyString_ReturnsNull()
		{
			var result = FieldHelper.GetCollectionGenericObject("");
			Assert.Null(result);
		}
	}
}