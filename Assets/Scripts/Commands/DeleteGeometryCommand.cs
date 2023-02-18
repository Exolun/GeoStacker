using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class DeleteGeometryCommand : ICommand
    {
        private GameObject geoToDelete;

        private GameObject geoPrototype;
        private Quaternion rotation;
        private Vector3 position;
        private Vector3 scale;
        private bool isUnion;

        public DeleteGeometryCommand(GameObject geoToDelete)
        {
            this.geoToDelete = geoToDelete;
        }

        public void Execute()
        {
            var properties = this.geoToDelete.GetComponent<GeometryProperties>();
            this.geoPrototype = ToolboxController.Instance.GetPrototypeByName(properties.GeometryType);
            this.isUnion = properties.IsUnion;


            this.rotation = new Quaternion(this.geoToDelete.transform.rotation.x, this.geoToDelete.transform.rotation.y, this.geoToDelete.transform.rotation.z, this.geoToDelete.transform.rotation.w);
            this.position = new Vector3(this.geoToDelete.transform.position.x, this.geoToDelete.transform.position.y, this.geoToDelete.transform.position.z);
            this.scale = new Vector3(this.geoToDelete.transform.localScale.x, this.geoToDelete.transform.localScale.y, this.geoToDelete.transform.localScale.z);

            GameObject.Destroy(this.geoToDelete);
            SelectionManager.Instance.ClearSelection();
            HierarchyController.Instance.DeleteGeometry(this.geoToDelete);
        }

        public void Undo()
        {
            
            // Run without adding to the stack for now
            // TODO: Implement Redo functionality
            var createCommand = new AddGeometryCommand(geoPrototype, position: this.position, rotation: this.rotation, scale: this.scale, materialToApply: isUnion ? null : ToolboxController.Instance.IntersectGeoMat);
            createCommand.Execute();
        }
    }
}
