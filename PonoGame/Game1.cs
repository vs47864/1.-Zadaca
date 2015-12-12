using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using PonoGame;

namespace PonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PonoGame : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Paddle PaddleBottom { get; private set; }
        public Paddle PaddleTop { get; private set; }
        public Ball Ball { get; private set; }
        public Background Background { get; private set; }
        public SoundEffect HitSound { get; private set; }
        public Song Music { get; private set; }
        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();
        public PonoGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var screenBounds = GraphicsDevice.Viewport.Bounds;
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");

            PaddleBottom = new Paddle(paddleTexture);
            PaddleTop = new Paddle(paddleTexture);

            PaddleBottom.Position = new Vector2(0, 900-PaddleBottom.Size.Height);
            PaddleTop.Position = new Vector2(0, 0);

            Texture2D ballTexture = Content.Load<Texture2D>("ball");

            Ball = new Ball(ballTexture);
            Ball.Position = screenBounds.Center.ToVector2();

            Texture2D backgroundTexture = Content.Load<Texture2D>("background");
            Background = new Background(backgroundTexture, screenBounds.Width, screenBounds.Height);

            HitSound = Content.Load<SoundEffect>("hit");
            //Music = Content.Load<Song>("proba");
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(Music);

            
            SpritesForDrawList.Add(Background);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(Ball);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var tochState = Keyboard.GetState();
            if (tochState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.Position.X -= (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            if (tochState.IsKeyDown(Keys.Right))
            {

                PaddleBottom.Position.X += (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (tochState.IsKeyUp(Keys.D))
            {
                PaddleTop.Position.X -= (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (tochState.IsKeyUp(Keys.A))
            {

                PaddleTop.Position.X += (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);

                // TODO: Add your update logic here
            }

            PaddleBottom.Position.X = MathHelper.Clamp(PaddleBottom.Position.X, graphics.GraphicsDevice.Viewport.Bounds.Left, graphics.GraphicsDevice.Viewport.Bounds.Right - PaddleBottom.Size.Width);

            PaddleTop.Position.X = MathHelper.Clamp(PaddleTop.Position.X, graphics.GraphicsDevice.Viewport.Bounds.Left, graphics.GraphicsDevice.Viewport.Bounds.Right - PaddleTop.Size.Width);

            Ball.Position += Ball.Direction * (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
            var bounds = graphics.GraphicsDevice.Viewport.Bounds;
            if( Ball.Position.X< bounds.Left || Ball.Position.X > bounds.Right - Ball.Size.Width)
            {
                Ball.Direction.X = -Ball.Direction.X;
                Ball.Speed = Ball.Speed * Ball.BumpSpeedIncreaseFactor;
                HitSound.Play();
            }

            if(CollisionDetector.Overlaps(Ball, PaddleTop) && Ball.Direction.Y<0 || (CollisionDetector.Overlaps(Ball, PaddleBottom) && Ball.Direction.Y > 0))
            {
                Ball.Direction.Y = -Ball.Direction.Y;
                Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
            }
                

            if (Ball.Position.Y> bounds.Bottom || Ball.Position.Y < bounds.Top)
            {
                Ball.Position = bounds.Center.ToVector2();
                Ball.Speed = Ball.InitialSpeed;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public abstract class Sprite
    {
        public Rectangle Size;

        public Vector2 Position;

        public Texture2D Texture { get; set; }

        protected Sprite(Texture2D spriteTexture, int width, int height)
        {
            Texture = spriteTexture;
            Size = new Rectangle(0, 0, width, height);
            Position = new Vector2(0, 0);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Size, Color.White);
        }
    }

    public class Background : Sprite
    {
        public Background(Texture2D spriteTexture, int width, int height)
                : base(spriteTexture, width, height)
        {
        }
    }

    public class Ball : Sprite
    {
        public const float InitialSpeed = 0.4f;

        public const float BumpSpeedIncreaseFactor = 1.01f;

        public const int BallSize = 50;

        public float Speed { get; set; }

        public Vector2 Direction;

        public Ball(Texture2D spriteTexture)
            : base(spriteTexture, BallSize, BallSize)
        {
            Speed = InitialSpeed;
            Direction = new Vector2(1, 1);
        }
    }

    public class Paddle : Sprite
    {
        private const float InitialSpeed = 0.9f;

        private const int PaddleHeight = 20;
        private const int PaddleWidth = 200;
        public float Speed { get; set; }
        public Paddle(Texture2D spriteTexture)
            : base(spriteTexture, PaddleWidth, PaddleHeight)
        {
            Speed = InitialSpeed;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Size, Color.Black);
        }
    }
}

public class CollisionDetector
{
    public static bool Overlaps(Sprite s1, Sprite s2)
    {
        if (s1.Position.X > s2.Position.X + s2.Size.Width || s1.Position.X + s1.Size.Width < s2.Position.X)
        {
            return false;
        }
        if (s1.Position.Y > s2.Position.Y + s2.Size.Height || s1.Position.Y + s1.Size.Height < s2.Position.Y)
        {
            return false;
        }
        return true;
    }
}
 