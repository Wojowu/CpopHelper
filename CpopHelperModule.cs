using System;
using Microsoft.Xna.Framework;
using Celeste.Mod.UI;
using FMOD.Studio;
using Monocle;
using Celeste;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Utils;
using Celeste.Mod.Entities;
using System.Security.Cryptography.X509Certificates;
using FMOD;
using Microsoft.Xna.Framework.Design;
using System.Threading;
using System.Drawing.Text;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace Celeste.Mod.CpopHelper {
    public class CpopHelperModule : EverestModule {
        public static CpopHelperModule Instance { get; private set; }

        public override Type SettingsType => typeof(CpopHelperModuleSettings);
        public static CpopHelperModuleSettings Settings => (CpopHelperModuleSettings)Instance._Settings;

        public override Type SessionType => typeof(CpopHelperModuleSession);
        public static CpopHelperModuleSession Session => (CpopHelperModuleSession)Instance._Session;

        public CpopHelperModule() {
            Instance = this;
#if DEBUG
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(CpopHelperModule), LogLevel.Verbose);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(CpopHelperModule), LogLevel.Info);
#endif
        }
        public override void Load() {

        }

        public override void Unload() {

        }

        [CustomEntity("setSubpixelTrigger")]
        public class SetSubpixelTrigger : Trigger {
            public float valueX, valueY;
            public bool setX, setY, ifStationary;
            private int stationaryFrames;

            public SetSubpixelTrigger(EntityData data, Vector2 offset) : base(data, offset)
            {
                valueX = data.Float("valueX");
                valueY = data.Float("valueY");
                setX = data.Bool("setX");
                setY = data.Bool("setY");
                ifStationary = data.Bool("ifStationary");
            }
            public override void OnEnter(Player player)
            {
                base.OnEnter(player);
                stationaryFrames = 0;
            }
            public override void OnStay(Player player)
            {
                base.OnStay(player);
                DynamicData playerData = DynamicData.For(player);
                Vector2 movementCounter = playerData.Get<Vector2>("movementCounter");
                Vector2 Speed = player.Speed;
                int State = player.StateMachine.State;
                bool isStationary = (Speed.X == 0f && Speed.Y == 0f && State != 2);
                if (isStationary)
                {
                    stationaryFrames++;
                } else
                {
                    stationaryFrames = 0;
                }
                if (!ifStationary || stationaryFrames > 2)
                {
                    if (setX)
                    {
                        movementCounter.X = valueX - 0.5f;
                    }
                    if (setY)
                    {
                        movementCounter.Y = 0.5f - valueY;
                    }
                }
                playerData.Set("movementCounter", movementCounter);
            }

        }

        [CustomEntity("checkSubpixelTrigger")]
        public class CheckSubpixelTrigger : Trigger
        {
            public float valueX, valueY, marginX, marginY;
            public bool checkX, checkY;
            public enum Action
            {
                KillOnIncorrect = 0,
                SetFlagOnCorrect = 1,
                SetFlagOnIncorrect = 2
            };
            public Action action;
            public string flag;
            public bool flagValue;

            public CheckSubpixelTrigger(EntityData data, Vector2 offset) : base(data, offset)
            {
                valueX = data.Float("valueX");
                valueY = data.Float("valueY");
                marginX = data.Float("marginX");
                marginY = data.Float("marginY");
                checkX = data.Bool("checkX");
                checkY = data.Bool("checkY");
                action = data.Enum<Action>("action", Action.KillOnIncorrect);
                flag = data.Attr("flag");
                flagValue = data.Bool("flagValue");
            }
            public override void OnEnter(Player player)
            {
                base.OnEnter(player);
                DynamicData playerData = DynamicData.For(player);
                Vector2 movementCounter = playerData.Get<Vector2>("movementCounter");
                float diffX = Math.Abs(movementCounter.X - (valueX - 0.5f));
                float diffY = Math.Abs(movementCounter.Y - (0.5f - valueY));
                if ((checkX && diffX > marginX) || (checkY && diffY > marginY))
                {
                    if (action == Action.KillOnIncorrect)
                    {
                        player.Die(-player.Speed.SafeNormalize());
                    }
                    if (action == Action.SetFlagOnIncorrect)
                    {
                        (Scene as Level).Session.SetFlag(flag, flagValue);
                    }
                }
                else
                {
                    if (action == Action.SetFlagOnCorrect)
                    {
                        (Scene as Level).Session.SetFlag(flag, flagValue);
                    }
                }
            }
        }

        [CustomEntity("cpopBlock")]
        public class cpopBlock : Entity
        {
            public int timer1, timer2, timer3;
            public enum Direction
            {
                Left = 0,
                Right = 1,
                Both = 2
            };
            public Direction direction;

            private int counter = 0;
            private int stage = 0;
            private int dirSign;

            public cpopBlock(EntityData data, Vector2 offset) : base(data.Position + offset)
            {
                base.Collider = new Hitbox(8f, 8f);
                timer1 = data.Int("timer1");
                timer2 = data.Int("timer2");
                timer3 = data.Int("timer3");
                direction = data.Enum<Direction>("direction");
            }

            public override void Update()
            {
                base.Update();
                Player player = Scene.Tracker.GetEntity<Player>();
                if (player == null)
                {
                    return;
                }
                DynamicData playerData = DynamicData.For(player);

                int playerPosX = (int)player.TopLeft.X;
                int playerPosY = (int)player.TopLeft.Y;
                int blockPosX = (int)this.BottomLeft.X;
                int blockPosY = (int)this.BottomLeft.Y;
                bool atCorner = false;
                if (playerPosX == blockPosX - 8 && (this.direction == Direction.Left || this.direction == Direction.Both))
                {
                    dirSign = 1;
                    if (playerPosY == blockPosY - 1 && player.StateMachine.State == 1)
                    {
                        atCorner = true;
                    }
                }
                else if (playerPosX == blockPosX + 8 && (this.direction == Direction.Right || this.direction == Direction.Both))
                {
                    dirSign = -1;
                    if (playerPosY == blockPosY - 1 && player.StateMachine.State == 1)
                    {
                        atCorner = true;
                    }
                }

                if (atCorner && (this.direction == Direction.Left || this.direction == Direction.Both))
                {
                    if (stage == 0)
                    {
                        stage = 1;
                    }
                }
                if (!Scene.Paused)
                {
                    counter++;
                }
                if (stage == 1)
                {
                    counter = 0;
                    if (stage == 1 && !Input.Grab.Check)
                    {
                        stage = 2;
                    }
                }
                if (stage == 2)
                {
                    player.Position.X = blockPosX + 4 - dirSign * 8;
                    player.Position.Y = blockPosY + 10;
                    player.Speed.Y = 0;
                    if (counter < timer1 && (Input.Grab.Check || Input.Jump.Check))
                    {
                        counter = 0;
                        stage = 0;
                    }
                    else if (counter >= timer1)
                    {
                        counter = 0;
                        stage = 3;
                    }
                }
                if (stage == 3)
                {
                    if (counter < timer2)
                    {
                        player.Position.X = blockPosX + 4 - dirSign * 8;
                        player.Position.Y = blockPosY + 11;
                        player.Speed.Y = 0;
                        if (Input.Grab.Check)
                        {
                            //playerData.Set("movementCounter", new Vector2((float)dirSign * 0.4f, 0f));
                            player.StateMachine.State = 1;
                            counter = 0;
                            stage = 4;
                        }
                    }
                    else
                    {
                        counter = 0;
                        stage = 0;
                    }
                }
                if (stage == 4)
                {
                    if (Input.Grab.Check && counter < timer3)
                    {
                        if (Input.Jump.Check && counter > 0)
                        {
                            player.StateMachine.State = 0;
                            counter = 0;
                            stage = 0;
                        }
                        else
                        {
                            playerData.Set("movementCounter", new Vector2((float)dirSign * 0.4f, 0f));
                            player.StateMachine.State = 1;
                            player.Position.X = blockPosX + 4 - dirSign * 8;
                            player.Position.Y = blockPosY + 11;
                        }
                    }
                    else
                    {
                        counter = 0;
                        stage = 0;
                    }
                }
                if (!atCorner)
                {
                    if (stage == 1)
                    {
                        stage = 0;
                    }
                }
            }
        }

        [Tracked]
        [CustomEntity("quizController")]
        public class quizController : Entity
        {
            public string controllerID;
            public string questionID;
            public int options;
            public int numberCorrect;
            public int numberIncorrect;
            public bool revealAnswer;
            public float displayScale;
            public enum AnswerType
            {
                Text = 0,
                Image = 1,
                HighResImage = 2
            };
            public AnswerType answerType;
            public enum Action
            {
                Kill = 0,
                Confetti = 1,
                SetFlagTrue = 2,
                SetFlagFalse = 3,
                None = 4,
            };
            public Action actionCorrect;
            public Action actionIncorrect;
            public string flag;

            public int value;
            public List<int> outputs;
            public List<Vector2> nodes;
            public bool guessedWrong = false;
            private int deathCount;
            private int seed;
            private int id;
            private bool answerRevealed;
            private List<MTexture> mTextures = new List<MTexture>();

            private static float camScale = 6f;
            private static Vector2 entityOffset = new(12f, 12f);

            public Random rnd;
            public quizController(EntityData data, Vector2 offset) : base(data.Position + offset)
            {
                base.Depth = 1000;
                base.Collider = new Hitbox(24f, 24f);
                id = data.ID;
                controllerID = data.Attr("controllerID");
                questionID = data.Attr("questionID");
                options = data.Int("options");
                numberCorrect = data.Int("numberCorrect");
                numberIncorrect = data.Int("numberIncorrect");
                revealAnswer = data.Bool("revealAnswer");
                answerType = data.Enum<AnswerType>("answerType");
                displayScale = data.Float("displayScale");
                actionCorrect = data.Enum<Action>("actionCorrect");
                actionIncorrect = data.Enum<Action>("actionIncorrect");
                flag = data.Attr("flag");
                nodes = data.NodesWithPosition(offset).Skip(1).ToList();
            }

            private void SetupQuiz()
            {
                value = rnd.Next(options);
                if (options > numberIncorrect + 1)
                {
                    outputs = Enumerable.Repeat(0, options).ToList();
                    for (int i = 0; i < options; i++)
                    {
                        outputs[i] = i == value ? rnd.Next(numberCorrect) : rnd.Next(numberIncorrect);
                    }
                }
                else
                {
                    outputs = Enumerable.Range(0, numberIncorrect).OrderBy(x => rnd.Next()).Take(options - 1).ToList();
                    outputs.Insert(value, rnd.Next(numberCorrect));
                }

                if (answerType != quizController.AnswerType.Text)
                {
                    for (int i = 0; i < options; i++)
                    {
                        string spritePath;
                        if (i == value)
                        {
                            spritePath = "quiz/" + questionID + "/correct/" + outputs[i].ToString() + "x";
                        }
                        else
                        {
                            spritePath = "quiz/" + questionID + "/incorrect/" + outputs[i].ToString() + "x";
                        }
                        mTextures.Add(GFX.Game[spritePath]);
                    }
                }
            }
            private void DrawImage(int i, Vector2 displayPos)
            {
                mTextures[i].DrawCentered(displayPos, Microsoft.Xna.Framework.Color.White, displayScale);
            }
            private void DrawText(bool correct, int answerID, Vector2 displayPos)
            {
                string textPath;
                if (correct)
                {
                    textPath = "quiz_" + questionID + "_correct_" + answerID.ToString();
                }
                else
                {
                    textPath = "quiz_" + questionID + "_incorrect_" + answerID.ToString();
                }
                string textLine = Dialog.Clean(textPath).ToString();

                ActiveFont.DrawOutline(textLine, displayPos, new Vector2 (0.5f,0.5f), displayScale * Vector2.One, Microsoft.Xna.Framework.Color.White, 2f, Microsoft.Xna.Framework.Color.Black);
            }

            public override void Added(Scene scene)
            {
                base.Added(scene);

                if (answerType != AnswerType.Image)
                {
                    Tag = TagsExt.SubHUD;
                }

                deathCount = (scene as Level).Session.Deaths;
                seed = id * id * id + deathCount * deathCount;
                var seedText = questionID + (Scene as Level).Session.LevelData.Name;
                foreach (char c in seedText)
                {
                    seed += (int)c;
                }
                if ((scene as Level).Session.GetFlag("quizVeryRandom"))
                {
                    seed += (int)Scene.TimeActive;
                }
                rnd = new Random(seed);

                SetupQuiz();
            }

            public override void Update()
            {
                base.Update();

                if (revealAnswer && value < nodes.Count() && guessedWrong && !answerRevealed)
                {
                    answerRevealed = true;
                    base.Scene.Add(new SummitCheckpoint.ConfettiRenderer(nodes[value] + entityOffset));
                }
            }
            public override void Render()
            {
                base.Render();

                Camera cam = SceneAs<Level>().Camera;
                for (int i = 0; i < Math.Min(options,nodes.Count); i++)
                {
                    Vector2 node = nodes[i];
                    Vector2 displayPos;
                    bool correct = i == value;
                    if (answerType == AnswerType.Image)
                    {
                        displayPos = node + entityOffset;
                    }
                    else
                    {
                        displayPos = camScale * (node - cam.Position) + camScale * entityOffset;
                    }

                    if (answerType == quizController.AnswerType.Text)
                    {
                        DrawText(correct, outputs[i], displayPos);
                    }
                    else
                    {
                        DrawImage(i, displayPos);
                    }
                }
            }

        }

        [CustomEntity("quizAnswerTrigger")]
        public class quizAnswerTrigger : Trigger
        {
            public string controllerID;
            public int index;
            public bool auto;

            private quizController controller;
            private bool correct = true;

            public quizAnswerTrigger(EntityData data, Vector2 offset) : base(data, offset)
            {
                index = data.Int("index");
                controllerID = data.Attr("controllerID");
                auto = data.Bool("auto");
            }

            public override void OnEnter(Player player)
            {
                base.OnEnter(player);

                foreach (quizController cont in Scene.Tracker.GetEntities<quizController>())
                {
                    if (cont != null && cont.controllerID == controllerID)
                    {
                        controller = cont;
                    }
                }

                if (controller == null)
                {
                    return;
                }

                if (auto)
                {
                    List<Vector2> nodes = controller.nodes;
                    Vector2 closestNode = nodes.ClosestTo(base.Center);
                    index = nodes.FindIndex(a => a == closestNode);
                }

                correct = (controller.value == index);

                if (correct)
                {
                    if (controller.actionCorrect == quizController.Action.Confetti)
                    {
                        base.Scene.Add(new SummitCheckpoint.ConfettiRenderer(player.Position));
                        Audio.Play("event:/game/07_summit/checkpoint_confetti", this.Position);
                    }
                    if (controller.actionCorrect == quizController.Action.SetFlagTrue)
                    {
                        (Scene as Level).Session.SetFlag(controller.flag, true);
                    }
                    if (controller.actionCorrect == quizController.Action.SetFlagFalse)
                    {
                        (Scene as Level).Session.SetFlag(controller.flag, false);
                    }
                }
                else
                {
                    if (controller.actionIncorrect == quizController.Action.Kill)
                    {
                        controller.guessedWrong = true;
                        player.Die(-player.Speed.SafeNormalize());
                    }
                    if (controller.actionIncorrect == quizController.Action.SetFlagTrue)
                    {
                        (Scene as Level).Session.SetFlag(controller.flag, true);
                    }
                    if (controller.actionIncorrect == quizController.Action.SetFlagFalse)
                    {
                        (Scene as Level).Session.SetFlag(controller.flag, false);
                    }
                }
            }
        }

        }

        [CustomEntity("HDGraphic")]
        public class HDGraphic : Entity
        {
            public string path;
            public float displayScale;

            private MTexture texture;

            private static float camScale = 6f;
            private static Vector2 entityOffset = new(12f, 12f);

            public HDGraphic(EntityData data, Vector2 offset) : base(data.Position + offset)
            {
                base.Depth = 1000;
                path = data.Attr("path");
                displayScale = data.Float("displayScale");
            }
            public override void Added(Scene scene)
            {
                base.Added(scene);
                Tag = TagsExt.SubHUD;

                texture = GFX.Game[path];
            }
            public override void Render()
            {
                base.Render();

                Camera cam = SceneAs<Level>().Camera;
                var displayPos = camScale * (this.Position - cam.Position) + camScale * entityOffset;
                texture.DrawCentered(displayPos, Microsoft.Xna.Framework.Color.White, displayScale);
            }

        }

}