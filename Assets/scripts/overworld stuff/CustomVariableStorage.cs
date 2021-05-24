

public class CustomVariableStorage : Yarn.Unity.VariableStorageBehaviour
{

    // Store a value into a variable
    public override void SetValue(string variableName, Yarn.Value value)
    {
        // 'variableName' is the name of the variable that 'value' 
        // should be stored in.

        // base.SetValue(variableName, value);
    }

    // Return a value, given a variable name
    public override Yarn.Value GetValue(string variableName)
    {
        // 'variableName' is the name of the variable to return a value for
        return new Yarn.Value();
    }

    // Return to the original state
    public override void ResetToDefaults()
    {

    }
}