var manageStories = {
    init: function () {
        manageStories.registerEvent();
    },
    registerEvent: function () {
        manageStories.loadData();
    },
    loadData: function () {
        $.ajax({
            url: '/Admin/StoriesAdmin/ManageStories',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    alert(res.status);
                }
            }
        })
    }
}
manageStories.init();