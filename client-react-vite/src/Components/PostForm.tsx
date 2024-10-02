export default function PostForm() {
    return (
        <>
        <div>
            <form>
                Ticker<input type="text" placeholder="Ticker symbol"/>
                Message<input type="text" placeholder="Message"/>
                Sentiment<input type="Sentiment" />
                <button type="submit">Post</button>
            </form>
        </div>
        </>
    )
}
