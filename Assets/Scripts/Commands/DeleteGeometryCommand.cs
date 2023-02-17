using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class DeleteGeometryCommand : ICommand
    {
        private GameObject geoToDelete;

        public DeleteGeometryCommand(GameObject geoToDelete)
        {
            this.geoToDelete = geoToDelete;
        }

        public void Execute()
        {
            GameObject.Destroy(this.geoToDelete);
            SelectionManager.Instance.ClearSelection();
            HierarchyController.Instance.DeleteGeometry(this.geoToDelete);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
