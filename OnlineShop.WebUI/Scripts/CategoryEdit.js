$(document).ready(function ()
{
    $(function ()
    {
        $("#MoveRight,#MoveLeft").click(function (event) {
            var id = $(event.target).attr("id");
            var selectFrom = id == "MoveRight" ? "#SelectedProducts" : "#AllProducts";
            var moveTo = id == "MoveRight" ? "#AllProducts" : "#SelectedProducts";
            var selectedProducts = $(selectFrom + " :selected").toArray();
            if (moveTo == "#SelectedProducts")
            {
                if (!isCategoryAdded(selectedProducts[0].value, moveTo))
                {
                    $(moveTo).append($(selectedProducts[0]).clone());
                }
            }
            else
            {
                selectedProducts[0].remove();
            }     
        });
    });  



});

function selectAllOptions(id)
{
    var selectedProducts = document.getElementById(id);
    for (i = 0; i < selectedProducts.options.length; i++)
        selectedProducts.options[i].selected = true;
}
function isCategoryAdded(categoryId, selectId)
{
    var selectedCategories = $(selectId);
    var options = selectedCategories.find('option');
    for (var i = 0; i < options.length; i++)
    {
        if (options[i].value == categoryId)
            return true;
    }
    return false;
}