using Microsoft.AspNetCore.Components.Forms;

namespace ScrumApp.Services;

public abstract partial class PostResponse
{
    public int MyProperty { get; set; }
    public void HandlePostFailure(EditContext editContext, ValidationMessageStore messageStore)
    {
        messageStore.Clear();

        if (this is UnprocessableEntity ue)
        {
            foreach (var error in ue.ValidationErrors)
            {
                messageStore.Add(editContext.Field(error.Key), error.Value);
            }
        }
        else
        {
            if (this is BadRequest br)
            {
                messageStore.Add(editContext.Field(""), br.ErrorMessage);
            }
            else// if (result is PostResponse.Failure)
            {
                messageStore.Add(editContext.Field(""), "There was a problem sending your request.");
            }
        }

        editContext.NotifyValidationStateChanged();
    }
}