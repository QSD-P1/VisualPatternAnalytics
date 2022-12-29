using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Detectors
{
	public class DetectCompositeUsecase : IDetectCompositeUsecase
	{
		public string PatternName => "Composite";

		public async Task<DetectorResultCollection> Detect(ProjectNode tree)
		{
			var resultCollection = new DetectorResultCollection()
			{
				Name = PatternName
			};


			// No classes
			if (tree.ClassNodes == null)
				return resultCollection;
			
			var allDistinctInterfaces = new List<string>();

			foreach (ClassNode classNode in tree.ClassNodes)
			{
				// Find interfaces
				if (classNode.Interfaces != null)
				{
					foreach (string classInterface in classNode.Interfaces)
					{
						if (allDistinctInterfaces.Contains(classInterface))
							continue;

						allDistinctInterfaces.Add(classInterface);
					}
				}
			}

			// No interfaces in use
			if (allDistinctInterfaces.Count == 0)
				return resultCollection;

			var classesPerInterface = new Dictionary<string, List<ClassNode>>();

			foreach (string distinctInterface in allDistinctInterfaces)
			{
				foreach (ClassNode classNode in tree.ClassNodes.Where(
					         x => x.Interfaces.Contains(distinctInterface)))
				{
					if (!classesPerInterface.ContainsKey(distinctInterface))
						classesPerInterface[distinctInterface] = new List<ClassNode>();

					classesPerInterface[distinctInterface].Add(classNode);
				}
			}


			return resultCollection;
		}
	}
}