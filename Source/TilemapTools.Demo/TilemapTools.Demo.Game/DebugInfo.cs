﻿using System;
using System.Collections.Generic;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Compositing;

namespace TilemapTools.Demo
{
    public class DebugInfo : SyncScript
    {

        public Keys ToggleKey = Keys.F12;

        public bool DebuggingEnabled = false;

        public TimeSpan MessageTimeOut = TimeSpan.FromSeconds(2);

        public Entity Camera;

        private FollowCamera FollowCameraController;
        private DebugCameraController DebugCameraController;

        private readonly Queue<string> messages = new Queue<string>();

        private TimeSpan elapsedTime = TimeSpan.Zero;
        private DelegateSceneRenderer delegateRenderer;
        private SpriteBatch spriteBatch;

        public SpriteFont Font { get; set; }

        public override void Start()
        {
            base.Start();

            var virtualResolution = new Vector3(GraphicsDevice.Presenter.BackBuffer.Width, GraphicsDevice.Presenter.BackBuffer.Height, 1);
            spriteBatch = new SpriteBatch(GraphicsDevice) { VirtualResolution = virtualResolution };

            GetSceneRendererCollection()?.Children.Add(delegateRenderer = new DelegateSceneRenderer(Draw));

            DebugCameraController = DebugCameraController ?? Camera?.Get<DebugCameraController>();
            FollowCameraController = FollowCameraController ?? Camera?.Get<FollowCamera>();
        }

        private SceneRendererCollection GetSceneRendererCollection()
        {
            var cameraRenderer = this.SceneSystem.GraphicsCompositor.Game as SceneCameraRenderer;
            var rendererCollection = cameraRenderer?.Child as SceneRendererCollection;
            return rendererCollection;
        }

        public override void Update()
        {
            if(Input.IsKeyPressed(ToggleKey))
            {
                DebuggingEnabled = !DebuggingEnabled;

                if (DebuggingEnabled)
                {
                    EnableDebugInfo();
                }
                else
                {
                    DisableDebugInfo();
                }                
            }

            elapsedTime += Game.UpdateTime.Elapsed;

            if(elapsedTime >= MessageTimeOut)
            {
                if(messages.Count > 0)
                    messages.Dequeue();

                elapsedTime -= MessageTimeOut;
            }
            
        }

        public void WriteMessage(string message) => messages.Enqueue(message);


        private void Draw(RenderDrawContext renderContext)
        {
            if(DebuggingEnabled)
            {
                if (Font != null)
                {
                    spriteBatch.Begin(renderContext.GraphicsContext, depthStencilState: DepthStencilStates.None);
                    DrawMessages(renderContext);

                    var size = spriteBatch.MeasureString(Font, $"FPS: {Game.UpdateTime.FramePerSecond:0.00}");
                    var position = new Vector2(spriteBatch.VirtualResolution.Value.X - size.X, 0);


                    spriteBatch.DrawString(Font, $"FPS: {Game.UpdateTime.FramePerSecond:0.00}", position, Color4.White);

                    spriteBatch.End();
                }
                    
            }
        }

        private void DrawMessages(RenderDrawContext renderContext)
        {
            var position = new Vector2();

            foreach (var message in messages)
            {
                spriteBatch.DrawString(Font, message, position, Color4.White);
                position.Y += spriteBatch.MeasureString(Font, message).Y;
            }            
        }

        public override void Cancel()
        {
            // Remove the delegate renderer from the pipeline

            SceneRendererCollection rendererCollection = GetSceneRendererCollection();
            rendererCollection?.Children.Remove(delegateRenderer);

            // destroy graphic objects
            spriteBatch.Dispose();
        }

        private void DisableDebugInfo()
        {
            var simulation = this.GetSimulation();
            if (simulation != null)
            {
                //simulation.ColliderShapesRendering = false;
            }
            
            if (DebugCameraController != null)
            {
                DebugCameraController.Reset();
                DebugCameraController.IsEnabled = false;
            }

            if (FollowCameraController != null)
            {
                FollowCameraController.IsEnabled = true;
            }
        }

        private void EnableDebugInfo()
        {
            var simulation = this.GetSimulation();
            if(simulation != null)
            {
                //simulation.ColliderShapesRendering = true;
            }

            if (FollowCameraController != null)
            {
                FollowCameraController.IsEnabled = false;
            }

            if (DebugCameraController != null)
            {
                DebugCameraController.Reset();
                DebugCameraController.IsEnabled = true;
            }
            
        }
    }
}
