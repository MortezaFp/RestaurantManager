(function ($) {
    $.fn.createOrder = function () {
        var filteredOrderItems;

        $('#submitOrderItems').click(function (e) {
            e.preventDefault();

            let orderItems = $('#table').bootstrapTable('getData', { unfiltered: true })
            filteredOrderItems = orderItems
                .map(({ id, name, price, quantity }) => ({ id, name, price: parseInt(price.replace(/,/g, "")), quantity }))
                .filter(({ quantity }) => quantity > 0);

            var totalPrice = 0;
            $("#OrderItemsPlaceholder").empty();
            $.each(filteredOrderItems, function (index, value) {
                var name = value.name;
                var price = value.price;
                var quantity = value.quantity;

                totalPrice += quantity * price;
                var $paragraph = $("<p>").text(quantity + " " + name + " for " + (quantity * price).toLocaleString() + "T");

                $("#OrderItemsPlaceholder").append($paragraph);
            });
            $("#OrderItemsPlaceholder").append($("<hr>"))
            $("#OrderItemsPlaceholder").append($("<p>").text("Total: " + totalPrice.toLocaleString() + "T"));

            var CreateOrderModal = new bootstrap.Modal($('#CreateOrderModal'));

            if (filteredOrderItems.length == 0) {
                $("#orderItemAlertPlaceHolder").text("Please select at least one food.").show();
            }
            else {
                $("#orderItemAlertPlaceHolder").hide()
                CreateOrderModal.toggle();
            }
        })

        $('#takeAwayAdditionalInputs').hide();

        $('#isTakeAway').on('click', function () {
            if ($(this).is(':checked')) {
                $('#takeAwayAdditionalInputs').show();
                $('#takeAwayAdditionalInputs').find('input').attr('required', true);
            } else {
                $('#takeAwayAdditionalInputs').hide();
                $('#takeAwayAdditionalInputs').find('input').attr('required', false);
            }
        })

        $("#CreateOrderForm").submit(function (e) {
            e.preventDefault();

            if ($("#CreateOrderForm").valid()) {
                var formData = $('#CreateOrderForm').find('input:not(.adjustment)').serializeArray().sort();

                if ($("#isTakeAway").is(':checked')) {
                    formData.pop();
                    formData.push(formData.splice(1, 1)[0]);
                }

                var adjustments = [];

                $('input[type=checkbox].adjustment').each(function () {
                    var checkboxName = $(this).attr('name');
                    var checkboxStatus = $(this).is(':checked');
                    if (checkboxStatus) {
                        adjustments.push({
                            id: checkboxName,
                            value: checkboxStatus
                        });
                    }
                });

                var postData = {
                    model: formData,
                    orderItems: filteredOrderItems,
                    adjustments: adjustments
                };

                $.ajax({
                    type: "POST",
                    url: "/Order/Home/Create",
                    data: { jsonInput: JSON.stringify(postData) },
                    success: function (result) {
                        console.log(result)
                        window.location = "/Order/Home/"
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr)
                        console.log(ajaxOptions)
                        console.log(thrownError)
                    }
                });
            }
        })
    }
})(jQuery)