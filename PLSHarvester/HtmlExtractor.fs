namespace PLSHarvester

open System
open System.IO
open HtmlAgilityPack
open System.Text.RegularExpressions


/// Extract HTML to plain text
module HtmlExtractor =
    /// Remove <script>, <style>, and unwanted <rt> nodes from HTML
    let private cleanDocument (doc: HtmlDocument) =

        // remove script/style safely
        let removeByName (name: string) =
            let nodes = doc.DocumentNode.SelectNodes($"//{name}")
            if nodes <> null then
                for n in nodes do n.Remove()

        removeByName "script"
        removeByName "style"

        // remove <rt> containing only hiragana/katakana
        let rtNodes = doc.DocumentNode.SelectNodes("//rt")
        if rtNodes <> null then
            let kanaOnly = Regex(@"^[\p{IsHiragana}\p{IsKatakana}[]+$")

            for rt in rtNodes do
                let text = rt.InnerText.Trim()
                if kanaOnly.IsMatch(text) then
                    rt.Remove()

    /// Clean HTML and return the text content
    let extractText (html: string) : string =
        let doc = HtmlDocument()
        doc.LoadHtml(html)

        cleanDocument doc

        doc.DocumentNode.InnerText
        |> fun s -> s.Replace("\u00A0", " ")
        |> fun s -> s.Trim()

    /// Given a file path, return the text content
    let loadHtmlFile (path: string) : string =
        File.ReadAllText(path) |> extractText
