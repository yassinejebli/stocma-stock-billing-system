export const uploadArticleImage = (id, file) => {

    const data = new FormData()
    data.append('file', file)
    data.append('id', id)
    data.append('type', 'Articles')

    return fetch('/UploadManager/UploadImage', {
        method: 'POST',
        body: data
    }).then(
        response => response.json()
    ).then(
        success => console.log(success)
    ).catch(
        error => console.log(error)
    );
};