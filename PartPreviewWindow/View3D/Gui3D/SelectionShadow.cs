﻿/*
Copyright (c) 2014, Lars Brubaker
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using MatterHackers.Agg;
using MatterHackers.Agg.Font;
using MatterHackers.Agg.Image;
using MatterHackers.Agg.Transform;
using MatterHackers.Agg.UI;
using MatterHackers.Agg.VertexSource;
using MatterHackers.DataConverters3D;
using MatterHackers.Localizations;
using MatterHackers.MeshVisualizer;
using MatterHackers.PolygonMesh;
using MatterHackers.RenderOpenGl;
using MatterHackers.RenderOpenGl.OpenGl;
using MatterHackers.VectorMath;
using System;

namespace MatterHackers.MatterControl.PartPreviewWindow
{
	public class SelectionShadow : InteractionVolume
	{
		static Mesh normalShadowMesh;
		static Color shadowColor = new Color(22, 80, 220);
		readonly int shadowAlpha = 40;


		public SelectionShadow(IInteractionVolumeContext context)
			: base(context)
		{
		}

		public override void SetPosition(IObject3D selectedItem)
		{
			AxisAlignedBoundingBox selectedBounds = selectedItem.GetAxisAlignedBoundingBox(Matrix4X4.Identity);
			Vector3 boundsCenter = selectedBounds.Center;

			TotalTransform = Matrix4X4.CreateTranslation(new Vector3(boundsCenter.X, boundsCenter.Y, 0.1));
		}

		Mesh GetNormalShadowMesh()
		{
			if(normalShadowMesh == null)
			{
				normalShadowMesh = PlatonicSolids.CreateCube(1, 1, .1);
			}

			return normalShadowMesh;
		}

		public override void DrawGlContent(DrawGlContentEventArgs e)
		{
			var selectedItem = RootSelection;
			if (selectedItem != null
				&& InteractionContext.Scene.ShowSelectionShadow)
			{
				// draw the bounds on the bed
				AxisAlignedBoundingBox selectedBounds = selectedItem.GetAxisAlignedBoundingBox(Matrix4X4.Identity);

				var withScale = Matrix4X4.CreateScale(selectedBounds.XSize, selectedBounds.YSize, 1) * TotalTransform;
				GLHelper.Render(GetNormalShadowMesh(), new Color(shadowColor, shadowAlpha), withScale, RenderTypes.Shaded);
			}

			base.DrawGlContent(e);
		}
	}
}