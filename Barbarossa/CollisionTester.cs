using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Barbarossa
{

    //    enum Quadrant
    //    {
    //        noMovement = 0,
    //        upperRight = 1,
    //        upperLeft,
    //        lowerLeft,
    //        lowerRight
    //    }

    class CollisionTester
    {
        /// <summary>
        /// Löst ein 2x2 lineares Gleichungssystem
        /// </summary>
        /// <param name="v">Der erste Richtungsvektor</param>
        /// <param name="p">Der erste Abschnittsvektor</param>
        /// <param name="w">Der zweite Richtungsvektor</param>
        /// <param name="q">Der zweite Abschnittsvektor</param>
        /// <returns>Der Lösungsvektor</returns>
        public static Vector2f LinSolve(Vector2f v, Vector2f p, Vector2f w, Vector2f q)
        {
            Vector2f b = q - p;
            w = -w;
            Vector2f a = new Vector2f();

            if (v.X == 0)
            {
                if (v.Y == 0 || w.X == 0)
                    throw new Exception("Keine oder unendliche viele Lösungen!");

                a.Y = b.X / w.X;
                a.X = (b.Y - a.Y * w.Y) / v.Y;
            }
            else
            {
                float r = v.Y / v.X;
                v.Y = 0;
                w.Y = w.Y - r * w.X;
                b.Y = b.Y - r * b.X;

                if (w.Y == 0)
                    throw new Exception("Keine oder unendliche viele Lösungen!");

                a.Y = b.Y / w.Y;
                a.X = (b.X - a.Y * w.X) / v.X;
            }
            return a;
        }

    static Collision TestCollision(IActiveCollider activeCollider, Vector2f passivePosition, Vector2f passiveSize, bool left, bool right, bool top, bool bot)
        {
            bool testLeft = false;
            bool testRight = false;
            bool testTop = false;
            bool testBot = false;
           // Quadrant activeMoveDirection = Quadrant.noMovement;

            if (activeCollider.ProposedMovement.X > 0)                                              //Check if direction and Side of collision match
            {
                testLeft = left;
                if (activeCollider.ProposedMovement.Y > 0)
                {
                    testTop = top;
                 //   activeMoveDirection = Quadrant.lowerRight;
                }
                else if (activeCollider.ProposedMovement.Y < 0)
                {
                    testBot = bot;
                  //  activeMoveDirection = Quadrant.upperRight;
                }
            }
            else if (activeCollider.ProposedMovement.X < 0)
            {
                testRight = right;
                if (activeCollider.ProposedMovement.Y > 0)
                {
                    testTop = top;
                  //  activeMoveDirection = Quadrant.lowerLeft;
                }
                else if (activeCollider.ProposedMovement.Y < 0)
                {
                    testBot = bot;
                //    activeMoveDirection = Quadrant.upperLeft;
                }
            }

            if (testLeft || testRight || testTop || testBot)
            {
                Vector2f passiveUpperLeft = passivePosition;
                Vector2f passiveLowerLeft = new Vector2f(passivePosition.X,passivePosition.Y + passiveSize.Y);
                Vector2f passiveUpperRight = new Vector2f(passivePosition.X + passiveSize.X, passivePosition.Y);
                Vector2f passiveLowerRight = new Vector2f(passivePosition.X + passiveSize.X, passivePosition.Y + passiveSize.Y);
                Vector2f activeOriginalUpperLeft = activeCollider.Position;
                Vector2f activeOriginalLowerLeft = new Vector2f(activeCollider.Position.X, activeCollider.Position.Y + activeCollider.Size.Y);
                Vector2f activeOriginalUpperRight = new Vector2f(activeCollider.Position.X + activeCollider.Size.X, activeCollider.Position.Y);
                Vector2f activeOriginalLowerRight = new Vector2f(activeCollider.Position.X + activeCollider.Size.X, activeCollider.Position.Y + activeCollider.Size.Y);

                if (testLeft)                                                                              //check if active has already passed passive
                {
                    testLeft = activeOriginalUpperRight.X < passiveUpperLeft.X;
                }
                if (testRight)
                {
                    testRight = activeOriginalUpperLeft.X > passiveUpperRight.X;
                }
                if (testTop)
                {
                    testTop = activeOriginalLowerLeft.Y < passiveUpperLeft.Y;
                }
                if (testBot)
                {
                    testBot = activeOriginalUpperLeft.Y > passiveLowerLeft.Y;
                }

                if (testLeft || testRight || testTop || testBot)
                {
                    Vector2f activeMovedUpperLeft = activeOriginalUpperLeft + activeCollider.ProposedMovement;
                    Vector2f activeMovedLowerLeft = activeOriginalLowerLeft + activeCollider.ProposedMovement;
                    Vector2f activeMovedUpperRight = activeOriginalUpperRight + activeCollider.ProposedMovement;
                    Vector2f activeMovedLowerRight = activeOriginalLowerRight + activeCollider.ProposedMovement;


                    if (testLeft || testRight)
                    {
                        bool outOfRange = activeOriginalLowerLeft.Y < passiveUpperLeft.Y && activeMovedLowerLeft.Y < passiveUpperLeft.Y;    //completely above?
                        outOfRange |= activeOriginalUpperLeft.Y > passiveLowerLeft.Y && activeMovedUpperLeft.Y > passiveUpperLeft.Y;        //completely below?
                        testLeft &= !outOfRange;
                        testRight &= !outOfRange;
                    }

                    if (testTop || testBot)
                    {
                        bool outOfRange = activeOriginalUpperRight.Y < passiveUpperLeft.Y && activeMovedUpperRight.Y < passiveUpperLeft.Y;    //completely on the left?
                        outOfRange |= activeOriginalUpperLeft.Y > passiveUpperRight.Y && activeMovedUpperLeft.Y > passiveUpperRight.Y;        //completely on the right?
                        testTop &= !outOfRange;
                        testBot &= !outOfRange;
                    }

                    if (testTop)
                    {
                        Vector2f lambdaTop;
                        Vector2f sideDirection = new Vector2f(passiveSize.X, 0);
                        int factor = (int)(Math.Abs(activeCollider.Size.X / passiveSize.X) + 1);
                        Vector2f activeSideSection = new Vector2f(activeCollider.Size.X / factor, 0);
                        for (int i = 0; i <= factor; i++)
                        {
                            lambdaTop = LinSolve(activeCollider.ProposedMovement, activeOriginalLowerLeft + i * activeSideSection, sideDirection, passiveUpperLeft);
                            if (lambdaTop.X >= 0 && lambdaTop.X <= 1 && lambdaTop.Y >= 0 && lambdaTop.Y <= 1)
                            {
                                Vector2f newMovement = activeCollider.ProposedMovement * lambdaTop.X;
                                return new Collision(newMovement, null, CollisionType.Top);
                            }
                        }
                    }

                    if (testLeft)
                    {
                        Vector2f lambdaLeft;
                        Vector2f sideDirection = new Vector2f(0, passiveSize.Y);
                        int factor = (int)(Math.Abs(activeCollider.Size.Y / passiveSize.Y)+1);
                        Vector2f activeSideSection = new Vector2f(0, activeCollider.Size.Y / factor);
                        for (int i = 0; i <= factor; i++)
                        {
                            lambdaLeft = LinSolve(activeCollider.ProposedMovement, activeOriginalUpperRight + i *activeSideSection , sideDirection, passiveUpperLeft);
                            if (lambdaLeft.X >= 0 && lambdaLeft.X <= 1 && lambdaLeft.Y >= 0 && lambdaLeft.Y <= 1)
                            {
                                Vector2f newMovement = activeCollider.ProposedMovement * lambdaLeft.X;
                                return new Collision(newMovement, null, CollisionType.Left);
                            }
                        }
                    }

                    if (testRight)
                    {
                        Vector2f lambdaRight;
                        Vector2f sideDirection = new Vector2f(0, passiveSize.Y);
                        int factor = (int)(Math.Abs(activeCollider.Size.Y / passiveSize.Y) + 1);
                        Vector2f activeSideSection = new Vector2f(0, activeCollider.Size.Y / factor);
                        for (int i = 0; i <= factor; i++)
                        {
                            lambdaRight = LinSolve(activeCollider.ProposedMovement, activeOriginalUpperLeft + i * activeSideSection, sideDirection, passiveUpperRight);
                            if (lambdaRight.X >= 0 && lambdaRight.X <= 1 && lambdaRight.Y >= 0 && lambdaRight.Y <= 1)
                            {
                                Vector2f newMovement = activeCollider.ProposedMovement * lambdaRight.X;
                                return new Collision(newMovement, null, CollisionType.Right);
                            }
                        }
                    }

                    if (testBot)
                    {
                        Vector2f lambdaBot;
                        Vector2f sideDirection = new Vector2f(passiveSize.X, 0);
                        int factor = (int)(Math.Abs(activeCollider.Size.X / passiveSize.X) + 1);
                        Vector2f activeSideSection = new Vector2f(activeCollider.Size.X / factor, 0);
                        for (int i = 0; i <= factor; i++)
                        {
                            lambdaBot = LinSolve(activeCollider.ProposedMovement, activeOriginalUpperLeft + i * activeSideSection, sideDirection, passiveLowerLeft);
                            if (lambdaBot.X >= 0 && lambdaBot.X <= 1 && lambdaBot.Y >= 0 && lambdaBot.Y <= 1)
                            {
                                Vector2f newMovement = activeCollider.ProposedMovement * lambdaBot.X;
                                return new Collision(newMovement, null, CollisionType.Bot);
                            }
                        }
                    }

                }
            }
            return null;
        }
    }
}
