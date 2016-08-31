using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.Drawing.Layout;
using VisioAutomation.Scripting.Utilities;
using VisioAutomation.Extensions;

namespace VisioAutomation.Scripting.Commands
{
    public class DistributeCommands : CommandSet
    {
        internal DistributeCommands(Client client) :
            base(client)
        {

        }

        public void DistributeOnAxis(Axis axis, double spacing)
        {
            if (!this._client.Document.HasActiveDocument)
            {
                return;
            }
            var application = this._client.Application.Get();
            var selection = this._client.Selection.Get();
            var shapeids = selection.GetIDs();
            using (var undoscope = this._client.Application.NewUndoScope("Distribute on Axis"))
            {
                var target = new TargetShapeIDs(application.ActivePage, shapeids);
                ArrangeHelper.DistributeWithSpacing(target, axis, spacing);
            }
        }

        public void DistributeHorizontal(TargetShapes targets, AlignmentHorizontal halign)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            int shape_count = targets.SetSelectionGetSelectedCount(this._client);
            if (shape_count < 1)
            {
                return;
            }

            IVisio.VisUICmds cmd;

            switch (halign)
            {
                case AlignmentHorizontal.Left:
                    cmd = IVisio.VisUICmds.visCmdDistributeLeft;
                    break;
                case AlignmentHorizontal.Center:
                    cmd = IVisio.VisUICmds.visCmdDistributeCenter;
                    break;
                case AlignmentHorizontal.Right:
                    cmd = IVisio.VisUICmds.visCmdDistributeRight;
                    break;
                default: throw new System.ArgumentOutOfRangeException();
            }

            var application = this._client.Application.Get();
            application.DoCmd((short)cmd);
        }

        public void DistributeVertical(TargetShapes targets, AlignmentVertical valign)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            int shape_count = targets.SetSelectionGetSelectedCount(this._client);
            if (shape_count < 1)
            {
                return;
            }

            IVisio.VisUICmds cmd;
            switch (valign)
            {
                case AlignmentVertical.Top:
                    cmd = IVisio.VisUICmds.visCmdDistributeTop;
                    break;
                case AlignmentVertical.Center: cmd = IVisio.VisUICmds.visCmdDistributeMiddle; break;
                case AlignmentVertical.Bottom: cmd = IVisio.VisUICmds.visCmdDistributeBottom; break;
                default: throw new System.ArgumentOutOfRangeException();
            }

            var application = this._client.Application.Get();
            application.DoCmd((short)cmd);
        }

        public void DistributeOnAxis(TargetShapes targets, Axis axis)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            int shape_count = targets.SetSelectionGetSelectedCount(this._client);
            if (shape_count < 1)
            {
                return;
            }


            IVisio.VisUICmds cmd;

            switch (axis)
            {
                case Axis.XAxis:
                    cmd = IVisio.VisUICmds.visCmdDistributeHSpace;
                    break;
                case Axis.YAxis:
                    cmd = IVisio.VisUICmds.visCmdDistributeVSpace;
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }

            var application = this._client.Application.Get();
            using (var undoscope = this._client.Application.NewUndoScope("Distribute Shapes"))
            {
                application.DoCmd((short)cmd);
            }
        }
    }
}