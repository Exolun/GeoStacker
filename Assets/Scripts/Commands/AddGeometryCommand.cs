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

        public AddGeometryCommand(GameObject newGeoPrototype, Material materialToApply = null)
        {
            this.newGeoPrototype = newGeoPrototype;
            this.materialToApply = materialToApply;
        }

        public void Execute()
        {
            var newGeometry = GameObject.Instantiate(this.newGeoPrototype);
            newGeometry.SetActive(true);
            newGeometry.transform.position = new Vector3(RandomNumberGenerator.GetInt32(-50, 50), RandomNumberGenerator.GetInt32(-50, 50), RandomNumberGenerator.GetInt32(-50, 50));

            if (this.materialToApply != null)
            {
                var renderer = newGeometry.GetComponent<Renderer>();
                renderer.sharedMaterial = this.materialToApply;
            }

            HierarchyController.Instance.AddGeometry(newGeometry);
            // SelectionManager.Instance.SetSelection(newGeometry);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
