import { React, useState, useRef } from "react";

const App = () => {
	const [selectedFile, setSelectedFile] = useState(null);
	const [response, setResponse] = useState(null);
	const linkRef = useRef(null);

	const onFileUpload = async () => {
		const formData = new FormData();
		formData.append("file", selectedFile);

		await fetch("https://localhost:7118/processFile", {
			method: "POST",
			body: formData,
		})
			.then((res) => {
				return res.json();
			})
			.then((data) => {
				console.log(data);
				setResponse(data);
			});
	};

	const onFileDownload = () => {
		console.log(response.file);

		try {
			const file = new Blob([response.file.fileContents], { type: response.file.contentType });
			console.log(file);
			const href = window.URL.createObjectURL(file);
			const anchor = linkRef.current;
			anchor.download = response.file.fileDownloadName;
			anchor.href = href;
			anchor.click();
			window.URL.revokeObjectURL(href);
		} catch (ex) {
			console.log("saveBlob method failed with the following exception:");
			console.log(ex);
		}
	};

	return (
		<div className="page">
			<div className="container">
				<div className="input__container">
					<input
						className="file__input"
						type="file"
						onChange={(e) => setSelectedFile(e.target.files[0])}
					/>
					<button className="button" onClick={() => onFileUpload()}>
						Process File
					</button>

					<button className="button" onClick={() => onFileDownload()}>
						Download File
					</button>
				</div>

				{response && (
					<>
						<p>
							Sum is {response.result} and was calculated in approximately {response.elapsedMs}{" "}
							milliseconds.
						</p>
						<a ref={linkRef}></a>
					</>
				)}
			</div>
		</div>
	);
};

export default App;
