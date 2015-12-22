using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace BarbarossaShared
{

    class CollisionTester
    {

        static public Collision TestCollision(IActiveCollider activeCollider, Vector2f proposedMovement, IPassiveCollider passiveCollider, bool left, bool right, bool top, bool bot)
        {
            Vector2f passivePosition = passiveCollider.Position;
            Vector2f passiveSize = passiveCollider.Size;
            bool testLeft = false;
            bool testRight = false;
            bool testTop = false;
            bool testBot = false;
            // Quadrant activeMoveDirection = Quadrant.noMovement;

            if (proposedMovement.X > 0)                                              //Check if direction and Side of collision match
            {
                testLeft = left;
            }
            else if (proposedMovement.X < 0)
            {
                testRight = right;
            }

            if (proposedMovement.Y > 0)
            {
                testTop = top;
            }
            else if (proposedMovement.Y < 0)
            {
                testBot = bot;
            }

            if (testLeft || testRight || testTop || testBot)
            {
                Vector2f passiveUpperLeft = passivePosition;
                Vector2f passiveLowerLeft = new Vector2f(passivePosition.X, passivePosition.Y + passiveSize.Y);
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
                    Vector2f activeMovedUpperLeft = activeOriginalUpperLeft + proposedMovement;
                    Vector2f activeMovedLowerLeft = activeOriginalLowerLeft + proposedMovement;
                    Vector2f activeMovedUpperRight = activeOriginalUpperRight + proposedMovement;
                    Vector2f activeMovedLowerRight = activeOriginalLowerRight + proposedMovement;


                    if (testLeft || testRight)
                    {
                        bool outOfRange = activeOriginalLowerLeft.Y < passiveUpperLeft.Y && activeMovedLowerLeft.Y < passiveUpperLeft.Y;    //completely above?
                        outOfRange |= activeOriginalUpperLeft.Y > passiveLowerLeft.Y && activeMovedUpperLeft.Y > passiveUpperLeft.Y;        //completely below?
                        testLeft &= !outOfRange;
                        testRight &= !outOfRange;
                    }

                    if (testTop || testBot)
                    {
                        bool outOfRange = activeOriginalUpperRight.X < passiveUpperLeft.X && activeMovedUpperRight.X < passiveUpperLeft.X;    //completely on the left?
                        outOfRange |= activeOriginalUpperLeft.X > passiveUpperRight.X && activeMovedUpperLeft.X > passiveUpperRight.X;        //completely on the right?
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
                            lambdaTop = BarbMath.LinSolve(proposedMovement, activeOriginalLowerLeft + i * activeSideSection, sideDirection, passiveUpperLeft);
                            if (lambdaTop.X >= 0 && lambdaTop.X <= 1 && lambdaTop.Y >= 0 && lambdaTop.Y <= 1)
                            {
                                Vector2f newMovement = proposedMovement * lambdaTop.X;
                                return new Collision(newMovement, passiveCollider, CollisionType.Top);
                            }
                        }
                    }

                    if (testLeft)
                    {
                        Vector2f lambdaLeft;
                        Vector2f sideDirection = new Vector2f(0, passiveSize.Y);
                        int factor = (int)(Math.Abs(activeCollider.Size.Y / passiveSize.Y) + 1);
                        Vector2f activeSideSection = new Vector2f(0, activeCollider.Size.Y / factor);
                        for (int i = 0; i <= factor; i++)
                        {
                            lambdaLeft = BarbMath.LinSolve(proposedMovement, activeOriginalUpperRight + i * activeSideSection, sideDirection, passiveUpperLeft);
                            if (lambdaLeft.X >= 0 && lambdaLeft.X <= 1 && lambdaLeft.Y >= 0 && lambdaLeft.Y <= 1)
                            {
                                Vector2f newMovement = proposedMovement * lambdaLeft.X;
                                return new Collision(newMovement, passiveCollider, CollisionType.Left);
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
                            lambdaRight = BarbMath.LinSolve(proposedMovement, activeOriginalUpperLeft + i * activeSideSection, sideDirection, passiveUpperRight);
                            if (lambdaRight.X >= 0 && lambdaRight.X <= 1 && lambdaRight.Y >= 0 && lambdaRight.Y <= 1)
                            {
                                Vector2f newMovement = proposedMovement * lambdaRight.X;
                                return new Collision(newMovement, passiveCollider, CollisionType.Right);
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
                            lambdaBot = BarbMath.LinSolve(proposedMovement, activeOriginalUpperLeft + i * activeSideSection, sideDirection, passiveLowerLeft);
                            if (lambdaBot.X >= 0 && lambdaBot.X <= 1 && lambdaBot.Y >= 0 && lambdaBot.Y <= 1)
                            {
                                Vector2f newMovement = proposedMovement * lambdaBot.X;
                                return new Collision(newMovement, passiveCollider, CollisionType.Bot);
                            }
                        }
                    }

                }
            }
            return null;
        }
    }
}
