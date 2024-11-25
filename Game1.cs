using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Bird-related variables
    private Vector2 _birdPosition;
    private Vector2 _birdVelocity;
    private const float Gravity = 0.5f;  // Constant for gravity effect
    private const float JumpStrength = -10f; // Constant for the jump strength (negative to move upwards)

    // Texture for the bird
    private Texture2D _birdTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Initialize bird's starting position and velocity
        _birdPosition = new Vector2(100, 200);  // Initial position of the bird
        _birdVelocity = Vector2.Zero; // Initial velocity is zero (no movement)

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Load the bird texture (make sure the file exists in the "Content" folder)
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _birdTexture = Content.Load<Texture2D>("bird");  // Load texture named "bird.png" (ensure it's in the Content folder)
    }

    protected override void Update(GameTime gameTime)
    {
        // Added the specific part you provided below:
        base.Update(gameTime); // Existing base call for game updates

        // Apply gravity effect: Increase the vertical velocity every frame to simulate gravity
        _birdVelocity.Y += Gravity;  // Adds gravity force to the bird's velocity (making it fall)

        // Update the bird's position based on its velocity (simple physics)
        _birdPosition += _birdVelocity;  // Moves the bird based on its velocity

        // Check for jump input (Spacebar): If pressed, the bird jumps upwards
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _birdVelocity.Y = JumpStrength;  // Set the velocity to a negative value (jump)
        }

        // Prevent the bird from falling off the screen
        if (_birdPosition.Y > _graphics.PreferredBackBufferHeight - _birdTexture.Height)
        {
            _birdPosition.Y = _graphics.PreferredBackBufferHeight - _birdTexture.Height;  // Keep the bird at the bottom of the screen
            _birdVelocity.Y = 0;  // Stop the bird from moving further downward
        }

        // Prevent the bird from flying above the screen
        if (_birdPosition.Y < 0)
        {
            _birdPosition.Y = 0;  // Reset bird's position to the top of the screen
            _birdVelocity.Y = 0;  // Stop upward movement when it hits the top
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the screen with a background color (Cornflower Blue)
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin drawing with SpriteBatch
        _spriteBatch.Begin();

        // Draw the bird texture at the updated position
        _spriteBatch.Draw(_birdTexture, _birdPosition, Color.White);

        // End drawing
        _spriteBatch.End();

        base.Draw(gameTime); 
    }
}
