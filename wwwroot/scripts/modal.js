const modal = document.getElementById('modal-foto');
const botaoAbrir = document.getElementById('acao-foto');
const botaoFechar = document.getElementById('fecharModalFoto');

// abre o modal
botaoAbrir.addEventListener('click', () => modal.showModal());

// fecha o modal
botaoFechar.addEventListener('click', () => modal.close());

// mostra a preview da foto
document.getElementById('upload-foto').addEventListener('change', e => {
    const arquivo = e.target.files[0];
    if (arquivo) {
        document.getElementById('preview-foto').src = URL.createObjectURL(arquivo);
    }
});
