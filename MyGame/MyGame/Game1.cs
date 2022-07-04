using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyGame
{
    public class Game1 : Game
    {
        SpriteBatch SpriteBatch { get; set; }

        Texture2D Spritesheet { get; set; }

        GraphicsDeviceManager Manager { get; set; }

        SpriteFont Font { get; set; }


        List<Bullet> Bullets = new List<Bullet>();
        List<Enemy> Enemies = new List<Enemy>();

        Random rnd = new Random();

        double rotation = 0;
        int total = 0;
        double reload = 0;
        bool gameover = false;

        public Game1()
        {
            Manager = new GraphicsDeviceManager(this);

        }

        protected override void Initialize()
        {
            Manager.PreferredBackBufferHeight = 1000;
            Manager.PreferredBackBufferWidth = 1000;
            Manager.ApplyChanges();
            using (var stream = TitleContainer.OpenStream("spritesheet.png"))
            {
                Spritesheet = Texture2D.FromStream(GraphicsDevice, stream);
            }

            Font = Content.Load<SpriteFont>("Font24");

            this.IsMouseVisible = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.LightGreen);
            SpriteBatch.Begin();

            DrawSprite(0, 0, 0, 0, 121, 207, (float)rotation, 1, 60);

            foreach (var bullet in Bullets)
            {
                DrawSprite(bullet.x, bullet.y, 0, 144, 7, 15, bullet.rotation, 2);
            }

            foreach (var enemy in Enemies)
            {
                if (enemy.type == 0)
                {
                    DrawSprite((int)enemy.x, (int)enemy.y, 0, 155, 60, 60, 0, 1);
                }
                else
                {
                    DrawSprite((int)enemy.x, (int)enemy.y, 0, 234, 60, 60, 0, 1);
                }
            }

            if (gameover)
            {
                DrawEndscreen();
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }
        private void DrawEndscreen()
        {
            string text = $"Points: {total}";
            int padding = 50;

            int width = (int)Font.MeasureString(text).X + padding;
            int height = (int)Font.MeasureString(text).Y + padding;

            var rect = new Texture2D(GraphicsDevice, width, height);

            Color c = Color.Pink;
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = c;
            rect.SetData(data);

            SpriteBatch.Draw(rect, new Vector2(500, 500) - Font.MeasureString(text) / 2 - new Vector2(padding, padding) / 2, c);
            SpriteBatch.DrawString(Font, text, new Vector2(500, 500) - Font.MeasureString(text) / 2, Color.White);
        }

        protected override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            if (gameover)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    rotation = 0;
                    total = 0;
                    reload = 0;
                    gameover = false;
                    Enemies.Clear();
                    Bullets.Clear();
                }
                return;
            }
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].x += (int)(Bullets[i].x * 0.05);
                Bullets[i].y += (int)(Bullets[i].y * 0.05);
                
                if (Bullets[i].x < -500 || Bullets[i].x > 500 || Bullets[i].y < -500 || Bullets[i].y > 500)
                {
                    Bullets.Remove(Bullets[i]);
                }
            }


            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].x += Enemies[i].speedAxisX;
                Enemies[i].y += Enemies[i].speedAxisY;
            }


            reload -= gameTime.ElapsedGameTime.TotalMilliseconds;
            reload = Math.Max(reload, 0);



            double rotationsPerSecond = 2 * Math.PI / 60;

            if (keyboard.IsKeyDown(Keys.A))
            {
                rotation -= rotationsPerSecond;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                rotation += rotationsPerSecond;
            }
            if (keyboard.IsKeyDown(Keys.Space) && reload == 0)
            {
                var bullet = new Bullet((int)(Math.Cos(rotation) * 147), (int)(Math.Sin(rotation) * 147), (float)rotation, (float)rotation);

                Bullets.Add(bullet);
                reload = 300;
            }

            if (rnd.Next(15) == 0)
            {
                int x = 0;
                int y = 0;
                while (true)
                {
                    x = rnd.Next(-1000, 1001);
                    y = rnd.Next(-1000, 1001);
                    if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) > 500 * Math.Sqrt(2))
                    {
                        break;
                    }
                }


                int type = rnd.Next(2);
                var enemy = new Enemy(x, y, type);
                Enemies.Add(enemy);
            }

            BulletsHitEnemies();
            EnemiesHitCannon();
        }

        private void EnemiesHitCannon()
        {
            foreach (var enemy in Enemies)
            {
                if (Math.Sqrt(Math.Pow(enemy.x, 2) + Math.Pow(enemy.y, 2)) < 60)
                {
                    gameover = true;
                    return;
                }
            }
        }

        private void BulletsHitEnemies()
        {
            foreach (var bullet in Bullets)
            {
                for (int i = 0; i < Enemies.Count; i++)
                {
                    if (Math.Sqrt(Math.Pow(bullet.x - Enemies[i].x, 2) + Math.Pow(bullet.y - Enemies[i].y, 2)) < 60)
                    {
                        Enemies.RemoveAt(i);
                        total += 1;
                    }
                }
            }
        }
        
        void DrawSprite(int drawX, int drawY, int imageX, int imageY, int imageHeight, int imageWidth, float rotation, float scale, int? xCenter = null, int? yCenter = null)
        {
            var center = new Vector2(xCenter ?? imageWidth / 2, yCenter ?? imageHeight / 2);

            SpriteBatch.Draw(Spritesheet, new Vector2(drawX + 500, drawY + 500), new Rectangle(imageX, imageY, imageWidth, imageHeight), Color.White, rotation, center, scale, SpriteEffects.None, 1);
        }
    }
}
