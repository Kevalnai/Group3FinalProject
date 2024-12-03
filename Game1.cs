using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Bird-related variables
    private Vector2 _birdPosition;
    private Vector2 _birdVelocity;
    private const float Gravity = 0.5f; // Constant for gravity effect
    private const float JumpStrength = -10f; // Constant for jump strength (negative to move upwards)
    private Texture2D _birdTexture;

    // Pipe-related variables
    private List<Rectangle> _pipes = new List<Rectangle>
    {
        new Rectangle(400, 300, 50, 200),
        new Rectangle(600, 200, 50, 250)
    };

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _birdPosition = new Vector2(100, 200); // Initial bird position
        _birdVelocity = Vector2.Zero;         // Initial velocity

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _birdTexture = Content.Load<Texture2D>("bird"); // Load bird texture from the Content folder
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
        // Jump logic
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _birdVelocity.Y = JumpStrength;
        }

        // Apply gravity
        _birdVelocity.Y += Gravity;
        _birdPosition += _birdVelocity;

        // Collision detection
        if (CheckCollision())
        {
            _birdVelocity = Vector2.Zero; // Stop movement on collision
        }

        // Prevent bird from leaving screen bounds
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

        // Draw bird
        _spriteBatch.Draw(_birdTexture, _birdPosition, Color.White);

        // Draw pipes
        foreach (var pipe in _pipes)
        {
            _spriteBatch.Draw(CreateTexture(pipe.Width, pipe.Height, Color.Green), pipe, Color.White);
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
