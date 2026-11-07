using UnityEngine;

public class RecipeBookTest : MonoBehaviour
{
    [SerializeField] private Recipe recipe;
    [SerializeField] private MachineData _machineData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Machine _machine = ItemFactory.CreateItemFromSO(_machineData) as Machine;
        print(recipe.ToString());
        GameManager.Instance.conveyorManager.MachineAdd?.Invoke(_machine);
        _machine.OnCraft?.Invoke(recipe, _machineData);
        _machine.OnCraft?.Invoke(recipe, _machineData);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
