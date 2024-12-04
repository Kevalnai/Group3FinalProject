// Flappy Bird
// Game
// Developed using MonoGame, this game replicates the mechanics of the classic Flappy Bird game. 
// Players control a bird that navigates through obstacles by jumping vertically. The bird gains altitude
// with a key press and is affected by gravity, requiring precise timing to pass through gaps in the pipes.
//
// Features:
// - Realistic gravity and jump mechanics.
// - Randomly generated pipe gaps to increase replayability.
// - A scoring system that rewards the player for successfully passing through pipes.
// - Sound effects that play when the bird successfully passes a pipe.
// - Start and restart functionality with keypress controls.
// - Visual prompts like "Press Enter to Start" and "Game Over" to guide the player.
//
// Controls:
// - Press Enter to start the game.
// - Press Space to make the bird jump.
// - Press R to restart the game after a game over.
//
// Objective:
// The goal is to achieve the highest possible score by passing through as many pipe gaps as possible
// without colliding with the pipes or the screen boundaries.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Group3FinalProject
{
  public Game1()
{
    _graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
}

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Vector2 _birdPosition;
        private Vector2 _birdVelocity;
        private const float Gravity = 0.5f;
        private const float JumpStrength = -7f;

        private List<Rectangle> _pipes = new List<Rectangle>();
        private const int PipeWidth = 50;
        private const int PipeGap = 150;
        private const int PipeSpeed = 5;

        private bool _gameStarted = false;
        private bool _gameOver = false;
        private int _score = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _birdPosition = new Vector2(100, 200);
            _birdVelocity = Vector2.Zero;
            InitializePipes();
            base.Initialize();
        }

        private void InitializePipes()
        {
            _pipes.Clear();
            int pipeY = new Random().Next(100, 300);
            _pipes.Add(new Rectangle(400, 0, PipeWidth, pipeY - PipeGap / 2));
            _pipes.Add(new Rectangle(400, pipeY + PipeGap / 2, PipeWidth, GraphicsDevice.Viewport.Height - (pipeY + PipeGap / 2)));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private bool CheckCollision()
        {
            foreach (var pipe in _pipes)
            {
                if (new Rectangle((int)_birdPosition.X, (int)_birdPosition.Y, 30, 30).Intersects(pipe))
                    return true;
            }
            return false;
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (!_gameStarted)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    _gameStarted = true;
                    _score = 0;
                }
                return;
            }

            if (_gameOver)
            {
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    _gameOver = false;
                    _score = 0;
                    Initialize();
                }
                return;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _birdVelocity.Y = JumpStrength;
            }

            _birdVelocity.Y += Gravity;
            _birdPosition += _birdVelocity;

            for (int i = 0; i < _pipes.Count; i++)
            {
                _pipes[i] = new Rectangle(_pipes[i].X - PipeSpeed, _pipes[i].Y, _pipes[i].Width, _pipes[i].Height);
            }

            if (_pipes[0].X < -PipeWidth)
            {
                _pipes.Clear();
                int newPipeY = new Random().Next(100, 300);
                _pipes.Add(new Rectangle(GraphicsDevice.Viewport.Width, 0, PipeWidth, newPipeY - PipeGap / 2));
                _pipes.Add(new Rectangle(GraphicsDevice.Viewport.Width, newPipeY + PipeGap / 2, PipeWidth, GraphicsDevice.Viewport.Height - (newPipeY + PipeGap / 2)));
                _score++;
            }

            if (CheckCollision())
            {
                _gameOver = true;
            }

            if (_birdPosition.Y > GraphicsDevice.Viewport.Height)
            {
                _birdPosition.Y = GraphicsDevice.Viewport.Height;
                _birdVelocity.Y = 0;
            }

            if (_birdPosition.Y < 0)
            {
                _birdPosition.Y = 0;
                _birdVelocity.Y = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(CreateTexture(30, 30, Color.Yellow), new Rectangle((int)_birdPosition.X, (int)_birdPosition.Y, 30, 30), Color.White);

            foreach (var pipe in _pipes)
            {
                _spriteBatch.Draw(CreateTexture(pipe.Width, pipe.Height, Color.Green), pipe, Color.White);
            }

            if (!_gameStarted)
            {
                _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont"), "Press Enter to Start", new Vector2(100, 200), Color.White);
            }
            else if (_gameOver)
            {
                _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont"), $"Score: {_score}", new Vector2(10, 10), Color.White);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont"), "Game Over! Press R to Restart", new Vector2(100, 200), Color.Red);
            }
            else
            {
                _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont"), $"Score: {_score}", new Vector2(10, 10), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Texture2D CreateTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            texture.SetData(data);
            return texture;
        }
    }
}
