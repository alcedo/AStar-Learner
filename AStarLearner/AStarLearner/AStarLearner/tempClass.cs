/*


// Get orientation 
                    foreach (Joint joint in data.Joints)
                    {
                        if (joint.ID == JointID.HandRight)
                            HandRightPos = joint.GetScreenPosition(kinectRuntime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                        
                        if (joint.ID == JointID.HandLeft)
                            HandLeftPos = joint.GetScreenPosition(kinectRuntime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                        
                        if (joint.ID == JointID.ElbowLeft)
                            ElbowLeft = joint.GetScreenPosition(kinectRuntime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                        if (joint.ID == JointID.ElbowRight)
                            ElbowRight = joint.GetScreenPosition(kinectRuntime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                     }

                    Vector2 rightOrientation = HandRightPos - ElbowRight;
                    Vector2 leftOrientation = HandLeftPos - ElbowLeft;

                    rightOrientation = Vector2.Multiply(rightOrientation, 1.5f);
                    leftOrientation = Vector2.Multiply(leftOrientation, 1.5f);
                    


                if (data.TrackingState == SkeletonTrackingState.Tracked)
                {
 
                    foreach (Joint joint in data.Joints)
                    {
                        if (joint.ID == JointID.HandRight || joint.ID == JointID.HandLeft)
                        {
                            // Console.WriteLine("width : " + rectangle.Width + " height : " + rectangle.Height);
                            Rectangle rectangle = new Rectangle();

                            // Place texture above the left and right hand
                            if (joint.ID == JointID.HandRight)
                            {
                                //Vector2 jointPosition = joint.GetScreenPosition(kinectRuntime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                                Vector2 jointPosition = rightOrientation;
                                rectangle = new Rectangle((int)jointPosition.X - (JointIntersectionSize / 2), 
                                    (int)jointPosition.Y - (JointIntersectionSize / 2), JointIntersectionSize, JointIntersectionSize);

                                skeletonSpots[0].Position = new Vector2((int)jointPosition.X, (int)jointPosition.Y);
                                debugSpots[0].Position = new Vector2(rectangle.X, rectangle.Y);
                            }
                            if (joint.ID == JointID.HandLeft)
                            {
                                Vector2 jointPosition = leftOrientation;
                                rectangle = new Rectangle((int)jointPosition.X - (JointIntersectionSize / 2),
                                 (int)jointPosition.Y - (JointIntersectionSize / 2), JointIntersectionSize, JointIntersectionSize);
                                
                                skeletonSpots[1].Position = new Vector2((int)jointPosition.X, (int)jointPosition.Y);
                                debugSpots[1].Position = new Vector2(rectangle.X, rectangle.Y);
                            }

                            foreach (GameTextureInstance texture in hotSpots)
                            {
                                if (texture.CalculateBoundingRectangle().Intersects(rectangle))
                                    texture.Color = Color.LimeGreen;
                            }
                        }
                    }
                }



*/