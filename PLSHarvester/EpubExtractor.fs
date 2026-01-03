namespace PLSHarvester

open System
open System.IO
open System.IO.Compression
open System.Text

module EpubExtractor =

    /// True if the entry looks like HTML or XHTML content file
    let private isHtmlEntry (entry: ZipArchiveEntry) =
        let name = entry.FullName.ToLowerInvariant()
        name.EndsWith(".html") || name.EndsWith(".xhtml") || name.EndsWith(".htm")

    /// Extract HTML/XHTML files inside EPUB and return their text content
    /// Use StringBuilder to avoid intermediate concatenation allocations.
    let extractTextFromEpub (epubPath: string) (htmlExtractor: string -> string) : string =
        use archive = ZipFile.OpenRead(epubPath)
        let sb = StringBuilder()
        let mutable first = true

        for entry in archive.Entries do
            if isHtmlEntry entry then
                try
                    use stream = entry.Open()
                    use reader = new StreamReader(stream)
                    let html = reader.ReadToEnd()
                    let text = htmlExtractor html
                    if not (String.IsNullOrWhiteSpace text) then
                        if not first then sb.Append("\n\n") |> ignore
                        sb.Append(text) |> ignore
                        first <- false
                with
                | _ -> () // optionally log

        sb.ToString()
