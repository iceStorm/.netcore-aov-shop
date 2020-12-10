
const request = require('request');
const cheerio = require('cheerio');
const { resolve } = require('path');
const { rejects } = require('assert');
const fs = require('fs');
const path = require('path');



async function start() {
    
    for (let i = 1; i <= 15; ++i) {
        let accounts = await doRequest(i);
        saveAccountToFile(i, accounts);
    }
}



function saveAccountToFile(index, accounts) {
    const folderPath = path.join(__dirname, '..', 'results', index + '');
    console.log(folderPath);

    fs.mkdir(folderPath, () => {
        let itemCounter = 1;
        for (let acc of accounts) {
            fs.writeFileSync(path.join(folderPath, `${itemCounter++}.json`), JSON.stringify(acc));
            // console.log('Done');
        }
    });
}


async function doRequest(currentPageIndex) {
    console.log(currentPageIndex);
    return new Promise((resolve, rejects) => {
        request({
            method: 'GET',
            url: `https://shopmobaviet.net/?page=${currentPageIndex}`
        },
        async (err, res, body) => {
    
            if (err || res.statusCode !== 200) {
                hasMorePage = false;
                console.log(err || body);
                return resolve();
            }
    
    
            const $ = cheerio.load(body, {decodeEntities: false});
            const accs = [];
    

            let counter = 0;
            let elems = $('.sa-lprow .sa-lpcol').toArray();
            for (let e of elems) {
                let accInfo = await getAccJson(currentPageIndex, counter++, e);
                accs.push(accInfo);
            }
    
    
            return resolve(accs);
        });
    });
}

async function getAccJson(currentPageIndex, counter, elem) {
    const $ = cheerio.load(elem, {decodeEntities: false});

    let userLoginName = `acc_${currentPageIndex < 10 ? '0' + currentPageIndex : currentPageIndex}`;
    userLoginName += `${counter < 10 ? '0' + counter : counter}`;
    let detailPhotos = await getDetailPhotos('https://shopmobaviet.net' + $('.sa-lpimglq').attr('href'));

    let acc = {
        loginName: userLoginName,
        rank: $('.sa-lpbpice').text().split('\n\n')[0].split(' LQ')[0].trim(),
        cash: $('.sa-cash').text().split(',').join('').split('đ')[0].trim(),
        heroes: $('.gg-lpbif .hero').text().replace('Tướng: ', '').trim(),
        skins: $('.gg-lpbif .skin').text().replace('Trang phục: ', '').trim(),
        golds: $('.gg-lpbpri .hero').text().replace('Vàng: ', '').replace(',', '').trim(),
        gemLevel: $('.gg-lpbpri .skin').text().replace('Điểm ngọc: ', '').trim(),
        thumbnailUrl: 'https://shopmobaviet.net' + $('.sa-lpping > img').attr('src'),
        detailPhotos: detailPhotos
    };
    

    return acc;
}

async function getDetailPhotos(url) {

    return new Promise((resolve, rejects) => {
        request({
            method: 'GET',
            url: url
        },
        async (err, res, body) => {
    
            if (err || res.statusCode !== 200) {
                hasMorePage = false;
                console.log(err || body);
                return resolve();
            }
    
    
            const $ = cheerio.load(body, {decodeEntities: false});
            let photos = $('img[alt="img champ"]').toArray();
            let photoUrls = [];
    
            for (let img of photos) {
                photoUrls.push('https://shopmobaviet.net' + img.attribs['src'].trim());
            }
    
    
            return resolve(photoUrls);
        });
    });
}



( async ()=> {

    await start();

})();