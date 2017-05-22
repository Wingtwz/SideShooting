using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    public class HelpScreen : Screen
    {
        public Rectangle backRect;

        private Texture2D bgImage;
        private SpriteFont titleFont, messageFont;
        private string message = "SideShooting es un juego en linea para dos jugadores de vista lateral.\n" +
            "Una vez conectados dos jugadores, la partida comenzara.\n" +
            "El objetivo es derrotar al jugador acertandole con disparos, y al mismo tiempo esquivar los del rival.\n" +
            "Cada jugador puede aguantar hasta 6 disparos antes de perder.\n" +
            "Los controles son:\n" +
            "W/A/S/D: Moverse hacia arriba/izquierda/abajo/derecha/n" +
            "Clic izquierdo: disparar en direccion al cursor del raton.\n" +
            "Clic derecho: aceleron - permite moverse mas rapido durante un corto espacio de tiempo.\n" +
            "Q: fogueo - permite borrar todos los proyectiles que existen en el mapa.\n\n" +
            "Ten en cuenta que aceleron solo estara disponible si la barra verde esta al maximo, y fogueo si lo esta la barra azul.\n" +
            "Ademas, solo podras tener un maximo de 9 proyectiles a la vez en el mapa, el numero restante se indica en la interfaz.";

        public HelpScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();
            
            backRect = new Rectangle(550, 620, 320, 80);
        }

        public override void LoadContent()
        {
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            messageFont = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(bgImage, new Vector2(0), Color.White);

            SpriteBatch.DrawString(titleFont, "Atras", new Vector2(backRect.X, backRect.Y), Color.White);
            SpriteBatch.DrawString(messageFont, message, new Vector2(70, 300), Color.White);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Help(this, gameTime);
            }
        }
    }
}
