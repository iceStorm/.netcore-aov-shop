const logger = require('morgan');
const app = require('express')();


app.use(logger('dev'));


const PORT = 5500;
app.listen(PORT, () => {
    console.log('Listening on port: ', PORT);
    console.log('Open browser on: http://localhost:' + PORT);
});


app.get('/', (req, res) => {
    res.status(200).send('Ok');
});
