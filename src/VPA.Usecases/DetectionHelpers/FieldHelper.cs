﻿using System.Collections.Generic;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class FieldHelper
	{
		public static bool ClassHasPublicStaticFieldWithOwnType(ClassNode node, out FieldNode leaf)
		{
			leaf = node.Children.OfType<FieldNode>()
			.Where(n =>
				   n.Modifiers.Contains(ModifiersEnum.Static)
				&& n.AccessModifier == AccessModifierEnum.Public
				&& n.Type == node.Name)
			.FirstOrDefault();

			return leaf != null;
		}
		public static bool HasFoundOtherClassFromFieldType(IEnumerable<ClassNode> classNodes, FieldNode fieldNode, ClassNode classNode, out ClassNode? matchedResult)
		{
			// initialize the out param
			matchedResult = null;

			// Return false because field type is same class
			if (fieldNode.Type == classNode.Name) return false;

			// Search for the classnode
			matchedResult = classNodes.FirstOrDefault(x => x.Name == fieldNode.Type);

			return matchedResult != null;
		}
	}
}