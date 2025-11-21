function loadComments(projectId) {
    fetch(`/ProjectManagement/Comments/${projectId}`)
        .then(res => res.json())
        .then(data => {
            const container = document.getElementById("comments-container");
            container.innerHTML = "";

            data.forEach(c => {
                const div = document.createElement("div");
                div.className = "comment-box mt-2 p-2 border rounded";

                div.innerHTML = `
                    <p>${c.content}</p>
                    <small>${new Date(c.createdDate).toLocaleString()}</small>
                `;

                container.append(div);
            });
        });
}

document.getElementById("sendComment").addEventListener("click", function () {

    const content = document.getElementById("commentInput").value;
    const projectId = Number(window.location.pathname.split('/').pop());

    fetch(`/ProjectManagement/Comments/add`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            content,
            projectId
        })
    })
        .then(res => res.json())
        .then(() => {
            document.getElementById("commentInput").value = "";
            loadComments(projectId);
        });
});
