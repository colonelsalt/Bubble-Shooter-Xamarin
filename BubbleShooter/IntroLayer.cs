using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace BubbleShooter
{
    public class IntroLayer : CCLayerColor
    {
        CCSprite playerBubble;
        CCSprite arrow;
        Random randomNumberGen;

        bool bubbleFired = false;
        bool clusterFormed = false;
        string[] colours = { "Blue.png", "Green.png", "Pink.png", "Red.png", "Yellow.png" };
        string[,] colourGrid;
        CCSprite[,] bubbleGrid;


        string playerColour;
        float touchAngle = 0;
        float velocityX;
        float velocityY;
        CCVector2 velocityVector = new CCVector2(0, 500);

        public IntroLayer() : base(CCColor4B.White)
        {
            randomNumberGen = new Random();
            int colourIndex = randomNumberGen.Next(0, 5);
            playerColour = colours[colourIndex];
            playerBubble = new CCSprite(playerColour);
            arrow = new CCSprite("arrow.png");

            playerBubble.PositionX = 768 / 2;
            playerBubble.PositionY = 30;

            arrow.PositionX = 768 / 2;
            arrow.PositionY = 90;

            CreateGrid();
            AddChild(arrow);
            AddChild(playerBubble);
            Schedule(MainGameLoop);
        }

        void CreateGrid()
        {
            colourGrid = new string[16,12];
            bubbleGrid = new CCSprite[16, 12];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    string newColour = colours[randomNumberGen.Next(0, 5)];
                    colourGrid[i, j] = newColour;
                    bubbleGrid[i, j] = new CCSprite(newColour);

                    bubbleGrid[i, j].PositionX = 32 + j * 64;
                    bubbleGrid[i, j].PositionY = 1024 + 32 - (i * 64);

                    AddChild(bubbleGrid[i, j]);
                }
            }

            for (int i = 8; i < 16; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    colourGrid[i, j] = "none";
                    bubbleGrid[i, j] = new CCSprite();
                    bubbleGrid[i, j].PositionX = -100;
                }
            }
        }

        void newPlayerBubble()
        {
            playerColour = colours[randomNumberGen.Next(0, 5)];
            playerBubble = new CCSprite(playerColour);
            playerBubble.PositionX = 768 / 2;
            playerBubble.PositionY = 30;
            bubbleFired = false;
            velocityX = 0;
            velocityY = 0;
            AddChild(playerBubble);
        }

        void MainGameLoop(float frameTime)
        {
            if (bubbleFired)
            {
                playerBubble.PositionX = playerBubble.PositionX + (velocityX * frameTime);
                playerBubble.PositionY = playerBubble.PositionY + (velocityY * frameTime);

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (playerBubble.BoundingBox.ContainsPoint(bubbleGrid[i, j].Position))
                        {

                            if (playerColour.Equals(colourGrid[i, j]))
                            {
                                deleteIfCluster(i, j);
                            }
                            else
                            {
                                playerBubble.PositionX = bubbleGrid[i, j].PositionX;
                                playerBubble.PositionY = bubbleGrid[i, j].PositionY - 64;
                                bubbleGrid[i + 1, j] = playerBubble;
                                colourGrid[i + 1, j] = playerColour;
                            }

                            newPlayerBubble();
                        }
                    }
                }
            }

            if (playerBubble.PositionY > 1024)
            {
                RemoveChild(playerBubble);
                newPlayerBubble();
            }
            else if (playerBubble.PositionX > 768 || playerBubble.PositionX < 0)
            {
                velocityX *= -1;
            }
            else if (playerBubble.PositionY < 0)
            {
                velocityY *= -1;
            }
        }

        void deleteIfCluster(int i, int j)
        {
            string colour = colourGrid[i, j];
            bool[,] matchingColourGrid = new bool[16, 12];

            for (int row = 0; row < 16; row++)
            {
                for (int column = 0; column < 12; column++)
                {
                    if (colourGrid[row, column].Equals(colour)) matchingColourGrid[row, column] = true;
                    else matchingColourGrid[row, column] = false;
                }
            }

            int[,] clusterIndeces = new int[5, 2];
            clusterIndeces[0, 0] = i;
            clusterIndeces[0, 1] = j;

            int clusterSize = 1;
            if (i - 1 > 0)
            {
                if (matchingColourGrid[i - 1, j])
                {
                    clusterIndeces[1, 0] = i - 1;
                    clusterIndeces[1, 1] = j;
                    clusterSize++;
                }
            }
            
            if (matchingColourGrid[i + 1, j])
            {
                clusterIndeces[2, 0] = i + 1;
                clusterIndeces[2, 1] = j;
                clusterSize++;
            }
            
            if (j + 1 < 12)
            {
                if (matchingColourGrid[i, j + 1])
                {
                    clusterIndeces[3, 0] = i;
                    clusterIndeces[3, 1] = j + 1;
                    clusterSize++;
                }
            }
            if (j - 1 > 0)
            {
                if (matchingColourGrid[i, j - 1])
                {
                    clusterIndeces[4, 0] = i;
                    clusterIndeces[4, 1] = j - 1;
                    clusterSize++;
                }
            }
            if (clusterSize >= 2)
            {
                for (int index = 0; index < 5; index++)
                {
                    i = clusterIndeces[index, 0];
                    j = clusterIndeces[index, 1];

                    RemoveChild(bubbleGrid[i, j]);
                    bubbleGrid[i, j] = new CCSprite();
                    colourGrid[i, j] = "none";
                    RemoveChild(playerBubble);
                }
            }
            else
            {
                playerBubble.PositionX = bubbleGrid[i, j].PositionX;
                playerBubble.PositionY = bubbleGrid[i, j].PositionY - 64;
                bubbleGrid[i + 1, j] = playerBubble;
                colourGrid[i + 1, j] = playerColour;
            }
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesEnded;
            touchListener.OnTouchesMoved = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling 
                if (playerBubble.BoundingBox.ContainsPoint(touches[0].Location) && !bubbleFired)
                {
                    velocityX = velocityVector.X;
                    velocityY = velocityVector.Y;
                    bubbleFired = true;
                }

                // Make sure the touch isn't at the same point as the arrow. If so,
                // then rotation cannot be calculated.
                else if (touches[0].Location.X != arrow.PositionX || touches[0].Location.Y != arrow.PositionY)
                {
                    float xOffset = touches[0].Location.X - arrow.PositionX;
                    float yOffset = touches[0].Location.Y - arrow.PositionY;

                    // Call Atan2 to get the radians representing the angle from 
                    // the arrow to the touch position
                    float radiansToTarget = (float)Math.Atan2(yOffset, xOffset);

                    // Since CCNode uses degrees for its rotation, we need to convert
                    // from radians
                    float degreesToTarget = CCMathHelper.ToDegrees(radiansToTarget);

                    // The direction that the entity faces when unrotated. In this case
                    // the entity is facing "up", which is 90 degrees 
                    const float forwardAngle = 90;

                    // Adjust the angle we want to rotate by subtracting the
                    // forward angle.
                    float adjustedForDirecitonFacing = degreesToTarget - forwardAngle;

                    // Invert the angle since CocosSharp uses clockwise rotation
                    touchAngle = adjustedForDirecitonFacing * -1;

                    // Finally assign the rotation
                    arrow.Rotation = touchAngle;

                    velocityVector = new CCVector2(0, 500);
                    RotateVector(ref velocityVector, touchAngle);
                }
            }
        }

        // Rotates the argument vector by degrees specified by
        // cocosSharpDegrees. In other words, the rotation
        // value is expected to be clockwise.
        // The vector parameter is modified, so it is both an in and out value
        void RotateVector(ref CCVector2 vector, float cocosSharpDegrees)
        {
            // Invert the rotation to get degrees as is normally
            // used in math (counterclockwise)
            float mathDegrees = -cocosSharpDegrees;

            // Convert the degrees to radians, as the System.Math
            // object expects arguments in radians
            float radians = CCMathHelper.ToRadians(mathDegrees);

            // Calculate the "up" and "right" vectors. This is essentially
            // a 2x2 matrix that we'll use to rotate the vector
            float xAxisXComponent = (float)System.Math.Cos(radians);
            float xAxisYComponent = (float)System.Math.Sin(radians);
            float yAxisXComponent = (float)System.Math.Cos(radians + CCMathHelper.Pi / 2.0f);
            float yAxisYComponent = (float)System.Math.Sin(radians + CCMathHelper.Pi / 2.0f);

            // Store the original vector values which will be used
            // below to perform the final operation of rotation.
            float originalX = vector.X;
            float originalY = vector.Y;

            // Use the axis values calculated above (the matrix values)
            // to rotate and assign the vector.
            vector.X = originalX * xAxisXComponent + originalY * yAxisXComponent;
            vector.Y = originalX * xAxisYComponent + originalY * yAxisYComponent;
        }
    }
}

