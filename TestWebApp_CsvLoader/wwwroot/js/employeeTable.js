$(document).ready(function () {
    // DataTables Initialization
    $.getJSON('/js/russia/ru.json', function (data) {
        $('#employeesTable').DataTable({
            "paging": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "language": data,
            "columnDefs": [
                { "orderable": false, "targets": -1 }
            ]
        });
    });

    $(document).on('click', '.edit-btn', function () {
        var $row = $(this).closest('tr');
        $row.find('span.editable').each(function () {
            var text = $(this).text();
            var columnName = $(this).data('column');
            $(this).html('<input type="text" class="form-control" name="' + columnName + '" value="' + text + '">');
        });
        $row.find('.edit-btn').addClass('d-none');
        $row.find('.save-btn').removeClass('d-none');
    });

    $(document).on('click', '.save-btn', function () {
        var row = $(this).closest('tr');
        var id = row.data('id');

        var employeeData = {
            LastName: row.find('input[name="LastName"]').val(),
            FirstName: row.find('input[name="FirstName"]').val(),
            Email: row.find('input[name="Email"]').val(),
            PhoneNumber: row.find('input[name="PhoneNumber"]').val(),
            Mobile: row.find('input[name="Mobile"]').val(),
            Address: row.find('input[name="Address"]').val(),
            Postcode: row.find('input[name="Postcode"]').val(),
            DateOfBirth: row.find('input[name="DateOfBirth"]').val(),
            StartDate: row.find('input[name="StartDate"]').val()
        };

        $.ajax({
            url: '/Employee/Update',
            method: 'POST',
            data: { id: id, updatedData: employeeData },
            success: function () {
                alert('Данные сотрудника успешно обновлены');
                row.find('input').each(function () {
                    var value = $(this).val();
                    $(this).parent().text(value);
                });
                row.find('.save-btn').addClass('d-none');
                row.find('.edit-btn').removeClass('d-none');
            },
            error: function () {
                alert('Произошла ошибка при обновлении данных сотрудника');
            }
        });
    });

    $(document).on('click', '.delete-btn', function () {
        var row = $(this).closest('tr');
        var employeeId = row.data('id');

        if (employeeId && employeeId !== 'new') {
            $.ajax({
                url: '/Employee/Delete',
                method: 'POST',
                data: { id: employeeId },
                success: function () {
                    alert('Сотрудник удален успешно');
                    row.remove();
                },
                error: function () {
                    alert('Произошла ошибка при удалении сотрудника');
                }
            });
        } else {
            row.remove();
        }
    });
});
