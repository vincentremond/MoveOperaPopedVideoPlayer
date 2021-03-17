using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutoMovePipWindow.Configuration;
using AutoMovePipWindow.DTO;
using Z.Expressions;

namespace AutoMovePipWindow.Services
{
    internal class ScreenConfigurationLocator
    {
        private readonly PipConfiguration _configuration;

        public ScreenConfigurationLocator(PipConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal List<PositionTargetAssociation> GetTargets(Screen[] allScreens)
        {
            var assoc = AssociateScreenAndPositions(allScreens);
            if (assoc == null)
            {
                return null;
            }

            return PositionTargetAssociations(assoc);
        }

        private List<PositionTargetAssociation> PositionTargetAssociations(ScreenPositions assoc)
        {
            var results = new List<PositionTargetAssociation>();
            foreach (var position in assoc.Positions)
            {
                var cursorScreenIndex = position.Cursor.Screen;
                var rectangle = GetRectangle(assoc.Screens[cursorScreenIndex].Bounds, position.Cursor);
                var targetScreen = assoc.Screens[position.Target.Screen];
                var targetPosition = position.Target.Position;
                var size = position.Target.Size ?? _configuration.Size;
                results.Add(new PositionTargetAssociation(rectangle, targetScreen, targetPosition, size));
            }

            return results;
        }

        private Rectangle GetRectangle(Rectangle screenBounds, CursorConfiguration positionCursor)
        {
            var x = (int) (screenBounds.X + (positionCursor.X * screenBounds.Width));
            var y = (int) (screenBounds.Y + (positionCursor.Y * screenBounds.Height));
            var width = (int) (positionCursor.Width * screenBounds.Width);
            var height = (int) (positionCursor.Height * screenBounds.Height);
            return new Rectangle(x, y, width, height);
        }

        private ScreenPositions AssociateScreenAndPositions(Screen[] allScreens)
        {
            foreach (var (_, screenConfiguration) in _configuration.Screens)
            {
                if (screenConfiguration.Conditions.Length != allScreens.Length)
                {
                    continue;
                }

                var matchedScreens = GetOrderedScreens(allScreens, screenConfiguration.Conditions);
                if (matchedScreens == null)
                {
                    continue;
                }

                return new ScreenPositions(matchedScreens, screenConfiguration.Positions);
            }

            return null;
        }

        private Screen[] GetOrderedScreens(Screen[] allScreens, ConditionsConfiguration[] conditions)
        {
            var screens = allScreens.ToList();
            var result = new List<Screen>();
            foreach (var condition in conditions)
            {
                var screen = screens.FirstOrDefault(s => EvaluateScreenAndConditions(s, condition));

                if (screen != null)
                {
                    screens.Remove(screen);
                    result.Add(screen);
                }
                else
                {
                    return null;
                }
            }

            if (screens.Count > 0)
            {
                throw new ApplicationException("should not happen");
            }

            return result.ToArray();
        }

        private bool EvaluateScreenAndConditions(Screen screen, ConditionsConfiguration condition)
        {
            if (condition.Primary.HasValue && condition.Primary.Value != screen.Primary)
            {
                return false;
            }

            if (condition.Expression != null && !Eval.Execute<bool>(condition.Expression, screen.Bounds))
            {
                return false;
            }

            return true;
        }
    }
}
