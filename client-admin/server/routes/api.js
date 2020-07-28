const express = require('express');
const router = express.Router();

/* GET api listing. */
router.get('/', (req, res) => {
  res.send('api works');
});

router.get('/config', (req, res) => {
  res.send('api config works');
});

module.exports = router;
