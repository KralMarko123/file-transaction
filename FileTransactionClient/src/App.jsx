import { React, useState, useRef } from "react";

const App = () => {
	const SERVER_PORT = 5001;
	const SERVER_URL = `https://localhost:${SERVER_PORT}`;
	const [selectedFile, setSelectedFile] = useState(null);
	const [message, setMessage] = useState(null);
	const linkRef = useRef(null);

	const onFileUpload = async () => {
		const formData = new FormData();
		formData.append("file", selectedFile);

		await fetch(`${SERVER_URL}/processFile`, {
			method: "POST",
			body: formData,
		})
			.then((response) => response.text())
			.then((message) => setMessage(message));
	};

	const onFileDownload = async () => {
		await fetch(`${SERVER_URL}/downloadFile`, {
			method: "GET",
		})
			.then((res) => res.blob())
			.then((fileData) => {
				const file = new Blob([fileData], { type: fileData.contentType });
				const href = window.URL.createObjectURL(file);
				const anchor = linkRef.current;
				anchor.download = "Result.txt";
				anchor.href = href;
				anchor.click();
				window.URL.revokeObjectURL(href);
			})
			.catch((error) => {
				console.error(error);
			});
	};

	return (
		<div className="page">
			<div className="container">
				<input
					className="file__input"
					type="file"
					onChange={(e) => setSelectedFile(e.target.files[0])}
				/>

				<div className="button__container">
					{selectedFile && (
						<button className="button" onClick={() => onFileUpload()}>
							Process File
						</button>
					)}

					{message && (
						<button className="button" onClick={() => onFileDownload()}>
							Download File
						</button>
					)}
				</div>

				{message && (
					<>
						<p>{message}</p>
						<a ref={linkRef}></a>
					</>
				)}
			</div>
		</div>
	);
};

export default App;
