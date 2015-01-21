var express = require('express'),
    router = express.Router();
var additionalInformation = require('./account/additional-information');

router.use('/additional-information', additionalInformation);

module.exports = router;
