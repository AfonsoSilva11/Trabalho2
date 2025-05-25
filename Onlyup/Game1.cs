using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace OnlyUp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D background;
        Texture2D platformTexture;
        Texture2D gameOverTexture;
        Texture2D[] playerSprites;

        Vector2 playerPosition;
        Vector2 playerVelocity;
        bool isOnGround = false;

        int currentFrame = 0;
        double animationTimer = 0;
        double frameSpeed = 0.2;

        List<Platform> platforms = new List<Platform>();
        Random random = new Random();

        const int PlayerWidth = 32;
        const int PlayerHeight = 32;

        const int PlatformWidth = 100;
        const int PlatformHeight = 20;

        int screenWidth;
        int screenHeight;

        float gravity = 0.4f;
        float jumpVelocity = -10f;

        SoundEffect jumpSound;
        SoundEffect deathSound;

        bool isGameOver = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenWidth = _graphics.PreferredBackBufferWidth = 480;
            screenHeight = _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            RestartGame();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/background.png"));
            platformTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/platform_wood.png"));
            gameOverTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/gameover.png"));

            playerSprites = new Texture2D[3];
            playerSprites[0] = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/player1.png"));
            playerSprites[1] = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/player2.png"));
            playerSprites[2] = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/player3.png"));

            jumpSound = SoundEffect.FromStream(File.OpenRead("Content/jump.wav"));
            deathSound = SoundEffect.FromStream(File.OpenRead("Content/death.wav"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (isGameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                    RestartGame();
                return;
            }

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left))
                playerPosition.X -= 3;
            if (kstate.IsKeyDown(Keys.Right))
                playerPosition.X += 3;

            playerVelocity.Y += gravity;

            if (isOnGround && kstate.IsKeyDown(Keys.Space))
            {
                playerVelocity.Y = jumpVelocity;
                isOnGround = false;
                jumpSound.Play();
            }

            playerPosition += playerVelocity;

            isOnGround = false;
            foreach (var platform in platforms)
            {
                platform.Update(gameTime);

                Rectangle playerRect = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, PlayerWidth, PlayerHeight);
                Rectangle platformRect = new Rectangle((int)platform.Position.X, (int)platform.Position.Y, PlatformWidth, PlatformHeight);

                if (playerRect.Intersects(platformRect) && playerVelocity.Y >= 0 &&
                    playerRect.Bottom <= platformRect.Top + 10)
                {
                    playerPosition.Y = platform.Position.Y - PlayerHeight;
                    playerVelocity.Y = 0;
                    isOnGround = true;
                    platform.Touch();
                }
            }

            float highestY = float.MaxValue;
            foreach (var p in platforms)
                if (p.Position.Y < highestY)
                    highestY = p.Position.Y;

            if (playerPosition.Y < highestY + 300)
            {
                float newY = highestY - 100;
                float newX = random.Next(50, screenWidth - PlatformWidth - 50);
                platforms.Add(new Platform(new Vector2(newX, newY)));
            }

            platforms.RemoveAll(p => p.Position.Y > playerPosition.Y + screenHeight);

            if (playerPosition.Y > screenHeight)
            {
                deathSound.Play();
                isGameOver = true;
            }

            if (playerPosition.Y < screenHeight / 2)
            {
                float offset = (screenHeight / 2) - playerPosition.Y;
                playerPosition.Y = screenHeight / 2;

                foreach (var platform in platforms)
                {
                    platform.Position = new Vector2(platform.Position.X, platform.Position.Y + offset);
                }
            }

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > frameSpeed)
            {
                currentFrame = (currentFrame + 1) % playerSprites.Length;
                animationTimer = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            foreach (var platform in platforms)
            {
                _spriteBatch.Draw(platformTexture,
                    new Rectangle((int)platform.Position.X, (int)platform.Position.Y, PlatformWidth, PlatformHeight),
                    Color.White);
            }

            _spriteBatch.Draw(playerSprites[currentFrame],
                new Rectangle((int)playerPosition.X, (int)playerPosition.Y, PlayerWidth, PlayerHeight),
                Color.White);

            if (isGameOver)
            {
                _spriteBatch.Draw(gameOverTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void RestartGame()
        {
            playerPosition = new Vector2(200, 600);
            playerVelocity = Vector2.Zero;
            platforms.Clear();
            platforms.Add(new Platform(new Vector2(200, 650)));
            isGameOver = false;
        }
    }

    public class Platform
    {
        public Vector2 Position;
        public bool IsTouched = false;
        public bool IsFalling = false;
        public float VelocityY = 2f;
        public double FallDelay = 1.0;
        private double timeSinceTouched = 0;

        public Platform(Vector2 pos)
        {
            Position = pos;
        }

        public void Update(GameTime gameTime)
        {
            if (IsTouched && !IsFalling)
            {
                timeSinceTouched += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceTouched >= FallDelay)
                    IsFalling = true;
            }

            if (IsFalling)
                Position = new Vector2(Position.X, Position.Y + VelocityY);
        }

        public void Touch()
        {
            if (!IsTouched)
            {
                IsTouched = true;
                timeSinceTouched = 0;
            }
        }
    }
}
