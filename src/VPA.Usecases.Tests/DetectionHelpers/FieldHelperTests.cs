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
				AccessModifier = AccessModifierEnum.Public
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
	}
}