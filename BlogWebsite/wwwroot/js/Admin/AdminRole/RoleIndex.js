var AdminRoleModule = (function () {
     
    function CreateRoleBtn() {
        $('#create-role-btn').on('click', function () {
            window.location.href = '/Admin/AdminRole/CreateRole';
        })
    }
    function SubmitCreateRole() {
        $('#create-role-submit').on('click', function () {
            window.location.href = '/Admin/AdminRole/CreateRole';
        })
    }
    function ToEditPage() {
        var id
        $('.to-edit-page').on('click', function () {
            id = $(this).data('id');
            window.location.href = '/Admin/AdminRole/EditRole?id='+id;
        })
    }

    function ToPermissionPage() {
        var roleId
        $('.to-permission-page').on('click', function () {
            roleId = $(this).data('roleid');
            window.location.href = '/Admin/AdminRole/GetPermissionForRole?roleId=' + roleId;
        })
    }

    function confirmDelete(uniqueId, isTrue) {
        var deleteSpan = 'deleteSpan_' + uniqueId;
        var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
        if (isTrue) {
            $('#' + deleteSpan).hide();
            $('#' + confirmDeleteSpan).show();

        }
        else {
            $('#' + deleteSpan).show();
            $('#' + confirmDeleteSpan).hide();
        }
    }

    return {
        "CreateRoleBtn": CreateRoleBtn,
        "SubmitCreateRole": SubmitCreateRole,
        "ToEditPage": ToEditPage,
        "confirmDelete": confirmDelete,
        "ToPermissionPage": ToPermissionPage
    }
})();