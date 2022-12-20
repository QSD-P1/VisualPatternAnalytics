using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;
using VPA.Domain.Enums;

namespace VPA.Usecases.DetectionHelpers
{
	public static class MethodHelper
	{
		public static bool HasSameClassReturnType(ClassNode classNode, out MethodNode leaf)
		{
			leaf = null;

			foreach (MethodNode methodNode in classNode.Children.OfType<MethodNode>())
			{
				if ((methodNode.Modifiers?.Contains(ModifiersEnum.Static) ?? false) && methodNode.ReturnType.Equals(classNode.Name))
				{
					leaf = methodNode;
					return true;
				}
			}
			return false;
		}
	}
}
