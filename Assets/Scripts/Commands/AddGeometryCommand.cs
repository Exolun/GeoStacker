using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    public class AddGeometryCommand : ICommand
    {
        private GameObject newGeoPrototype;
        private Material materialToApply;
        private Quaternion? rotation;
        private Vector3? position;
        private Vector3? scale;

        private GameObject geometryCreated;

        public AddGeometryCommand(GameObject newGeoPrototype, Material materialToApply = null, Quaternion? rotation = null, Vector3? position = null, Vector3? scale = null)
        {
            this.newGeoPrototype = newGeoPrototype;
            this.materialToApply = materialToApply;
            
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }

        public void Execute()
        {
            var newGeometry = GameObject.Instantiate(this.newGeoPrototype);
            newGeometry.SetActive(true);
            if (this.position == null)
            {
                newGeometry.transform.position = new Vector3(RandomNumberGenerator.GetInt32(-50, 50), RandomNumberGenerator.GetInt32(-50, 50), RandomNumberGenerator.GetInt32(-50, 50));
            }
            else
            {
                newGeometry.transform.position = (Vector3)this.position;
                newGeometry.transform.localScale = (Vector3)this.scale;
                newGeometry.transform.rotation = (Quaternion)this.rotation;
            }


            if (this.materialToApply != null)
            {
                var renderer = newGeometry.GetComponent<Renderer>();
                renderer.sharedMaterial = this.materialToApply;
                newGeometry.GetComponent<GeometryProperties>().IsUnion = false;
            }

            HierarchyController.Instance.AddGeometry(newGeometry);
            this.geometryCreated = newGeometry;
        }

        public void Undo()
        {
            // Delete it without adding to the event stack for now
            // TODO: Implement Redo functionality
            var deleteGeo = new DeleteGeometryCommand(this.geometryCreated);
            deleteGeo.Execute();
        }
    }
}

