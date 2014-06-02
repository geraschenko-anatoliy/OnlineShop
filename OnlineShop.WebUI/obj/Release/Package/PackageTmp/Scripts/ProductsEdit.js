$(document).ready(function () {
    $(function () {
        $("#MoveRight,#MoveLeft").click(function (event) {
            var id = $(event.target).attr("id");
            var selectFrom = id == "MoveRight" ? "#SelectedCategories" : "#AllCategories";
            var moveTo = id == "MoveRight" ? "#AllCategories" : "#SelectedCategories";
            var selectedCategories = $(selectFrom + " :selected").toArray();
            if (moveTo == "#SelectedCategories")
            {
                if (!isProductAdded(selectedCategories[0].value, moveTo))
                {
                    $(moveTo).append($(selectedCategories[0]).clone());
                }
            }else {
                selectedCategories[0].remove();
            }        
        });
    });  
});


function selectAllOptions(id) {
            var ref = document.getElementById(id);

            for (var i = 0; i < ref.options.length; i++)
                ref.options[i].selected = true;
    }

function isProductAdded(productId, selectId) {
    var selectedProducts = $(selectId);
    var options = selectedProducts.find('option');
    for (var i = 0; i < options.length; i++)
    {
        if (options[i].value == productId)
            return true;
    }
    return false;
}