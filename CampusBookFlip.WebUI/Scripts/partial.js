function Partial(options) {
    if ('type' in options) {
        this.type = options.type;
    } else {
        this.type = 'POST';
    }
    if ('url' in options) {
        this.url = options.url;
    }
    //if ('contentType' in options) {
    //    this.contentType = options.contentType;
    //} else {
    //    this.contentType = 'application/json; charset=utf-8';
    //}
    if ('data' in options) {
        this.data = options.data;
    }
    if ('headers' in options) {
        this.headers = options.headers;
    } else {
        this.headers = AddAntiForgeryToken({});
    }
    //if ('dataType' in options) {
    //    this.dataType = options.dataType;
    //}
    //else {
    //    this.dataType = 'json';
    //}
    if ('failure' in options) {
        this.failure = options.failure;
    } else {
        this.failure = function (data) {
            console.log('Partial failure');
            console.log(data);
        };
    }
}

Partial.prototype.partial = function (callback) {
    $.ajax({
        type: this.type,
        url: this.url,
        contentType: this.contentType,
        data: this.data,
        headers: this.headers,
        dataType: this.dataType,
        success: function (data) {
            console.log('success');
            callback(data);
        },
        failure: function (data) {
            console.log('failure ' + data);
            this.failure(data);
        }
    });
}