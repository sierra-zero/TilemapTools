﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko
{
    public class CachedQuadBatchRenderer
    {
        private EffectInstance DefaultEffect;

        private EffectInstance Effect => DefaultEffect;
        private ObjectParameterAccessor<Texture>? textureUpdater;
        private ObjectParameterAccessor<SamplerState>? samplerUpdater;

        private MutablePipelineState MutablePipeline;
        private GraphicsDevice GraphicsDevice;
        private GraphicsContext GraphicsContext;
        private Matrix ViewProjectionMatrix;
        private bool hasBegun;

        public ParameterCollection Parameters => Effect.Parameters;

        public CachedQuadBatchRenderer(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException(nameof(graphicsDevice));

            GraphicsDevice = graphicsDevice;
            MutablePipeline = new MutablePipelineState(graphicsDevice);
            DefaultEffect = new EffectInstance(new Effect(GraphicsDevice, SpriteEffect.Bytecode){ Name = "BatchDefaultEffect"});
        }

        private void PrepareForRendering()
        {
            var localSamplerState =  GraphicsDevice.SamplerStates.PointClamp;

            // Sets the sampler state of the effect
            if (samplerUpdater.HasValue)
                Parameters.Set(samplerUpdater.Value, localSamplerState);

            Effect.UpdateEffect(GraphicsDevice);

            // Setup states (Blend, DepthStencil, Rasterizer)
            MutablePipeline.State.SetDefaults();
            MutablePipeline.State.RootSignature = Effect.RootSignature;
            MutablePipeline.State.EffectBytecode = Effect.Effect.Bytecode;
            MutablePipeline.State.BlendState = BlendStates.AlphaBlend;
            MutablePipeline.State.DepthStencilState = DepthStencilStates.Default;
            MutablePipeline.State.RasterizerState = RasterizerStates.CullBack;
            MutablePipeline.State.InputElements = VertexPositionTexture.Layout.CreateInputElements();
            MutablePipeline.State.PrimitiveType = PrimitiveType.TriangleList;
            MutablePipeline.State.Output.CaptureState(GraphicsContext.CommandList);
            MutablePipeline.Update();

        }

        public void Begin(GraphicsContext graphicsContex, Matrix viewProjection)
        {
            CheckEndHasBeenCalled();

            GraphicsContext = graphicsContex;
            ViewProjectionMatrix = viewProjection;

            textureUpdater = null;
            if (Effect.Effect.HasParameter(TexturingKeys.Texture0))
                textureUpdater = Effect.Parameters.GetAccessor(TexturingKeys.Texture0);

            samplerUpdater = null;
            if (Effect.Effect.HasParameter(TexturingKeys.Sampler))
                samplerUpdater = Effect.Parameters.GetAccessor(TexturingKeys.Sampler);

            PrepareForRendering();

            MutablePipeline.State.Output.CaptureState(GraphicsContext.CommandList);
            MutablePipeline.Update();

            GraphicsContext.CommandList.SetPipelineState(MutablePipeline.CurrentState);

            hasBegun = true;
        }
        
        public void Draw(CachedQuadBatch batch, Matrix world)
        {
            CheckBeginHasBeenCalled();

            var wvp = world * ViewProjectionMatrix;

            Effect.Apply(GraphicsContext);
            Parameters.Set(SpriteBaseKeys.MatrixTransform, wvp);

            GraphicsContext.CommandList.SetVertexBuffer(0, batch.VertexBuffer, 0, batch.VertexSize);
            GraphicsContext.CommandList.SetIndexBuffer(batch.IndexBuffer, 0, false);

            foreach (var range in batch.Ranges)
            {
                if (textureUpdater.HasValue)
                    Parameters.Set(textureUpdater.Value, range.Texture);

                GraphicsContext.CommandList.DrawIndexed(range.IndexCount, range.StartIndex);
            }
        }

        public void End()
        {
            CheckBeginHasBeenCalled();

            hasBegun = false;
        }

        private void CheckBeginHasBeenCalled([CallerMemberName] string caller = null)
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before " + caller);
            }
        }

        private void CheckEndHasBeenCalled([CallerMemberName] string caller = null)
        {
            if (hasBegun)
            {
                throw new InvalidOperationException("End must be called before " + caller);
            }
        }
    }
}
